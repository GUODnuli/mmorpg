using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using System.Diagnostics;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

        public void Init()
        {

        }

        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User: {0}, Password: {1}", request.User, request.Password);
            sender.Session.Response.userLogin = new UserLoginResponse();
            
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "该用户未注册，请注册后重新登陆.";
            }
            else if (user.Password != request.Password)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误！";
            }
            else
            {
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "登陆成功！";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach(var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    info.EntityId = c.TID;
                    info.Type = CharacterType.Player;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }
            }

            sender.SendResponse();
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User: {0}, Password: {1}", request.User, request.Password);

            sender.Session.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                sender.Session.Response.userRegister.Result = Result.Failed;
                sender.Session.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Password, Player = player });
                DBService.Instance.Entities.SaveChanges();
                sender.Session.Response.userRegister.Result = Result.Success;
                sender.Session.Response.userRegister.Errormsg = "创建成功！";
            }

            sender.SendResponse();
        }

        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacter: Name: {0}, CharacterClass: {1}", request.Name, request.Class);

            sender.Session.Response.createChar = new UserCreateCharacterResponse();

            try
            {
                TCharacter addCharacter = new TCharacter()
                {
                    Name = request.Name,
                    Class = (int)request.Class,
                    TID = (int)request.Class,
                    MapID = 1,
                    MapPosX = 5000,
                    MapPosY = 4000,
                    MapPosZ = 820,
                    Gold = 100000,
                    Equips = new byte[28]
                };
                var bag = new TCharacterBag();
                bag.Owner = addCharacter;
                bag.Items = new byte[0];
                bag.Unlocked = 20; 
                TCharacterItem it = new TCharacterItem();
                addCharacter.Bag = DBService.Instance.Entities.CharacterBags.Add(bag);
                addCharacter = DBService.Instance.Entities.Characters.Add(addCharacter);

                // 创建角色时添加道具
                addCharacter.Items.Add(new TCharacterItem()
                {
                    Owner = addCharacter,
                    ItemID = 1,
                    ItemCount = 20,
                });
                addCharacter.Items.Add(new TCharacterItem()
                {
                    Owner = addCharacter,
                    ItemID = 2,
                    ItemCount = 20,
                });
                sender.Session.User.Player.Characters.Add(addCharacter);
                DBService.Instance.Entities.SaveChanges();

                foreach (var character in sender.Session.User.Player.Characters)
                {
                    NCharacterInfo nCharacterInfo = new NCharacterInfo
                    {
                        Id = 0,
                        Name = character.Name,
                        Class = (CharacterClass)character.Class,
                        EntityId = character.ID,
                        Type = CharacterType.Player,
                    };
                    sender.Session.Response.createChar.Characters.Add(nCharacterInfo);
                }
                sender.Session.Response.createChar.Result = Result.Success;
                sender.Session.Response.createChar.Errormsg = "创建成功！";
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("保存新角色失败: {0}", ex.Message);
                sender.Session.Response.createChar.Result = Result.Failed;
                sender.Session.Response.createChar.Errormsg = "创建失败！";
            }

            sender.SendResponse();
        }

        void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: CharacterID: {0}, CharacterName: {1}, Map: {0}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);

            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "成功进入游戏世界！";
            sender.Session.Response.gameEnter.Character = character.Info;

            sender.SendResponse();

            sender.Session.Character = character;
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            try
            {
                Character character = sender.Session.Character;
                Log.InfoFormat("UserGameLeaveRequest: characterID: {0}, CharacterName: {1}, MapID: {2}", character.EntityId, character.Info.Name, character.Info.mapId);
                UserLeaveGame(character);

                sender.Session.Response.gameLeave.Result = Result.Success;
                sender.Session.Response.gameLeave.Errormsg = "离开游戏成功！";
            }
            catch (Exception e)
            {
                sender.Session.Response.gameLeave.Result = Result.Failed;
                sender.Session.Response.gameLeave.Errormsg = string.Format("离开游戏失败！失败原因：{0}", e);
            }

            sender.SendResponse();
        }

        public void UserLeaveGame(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.EntityId);
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }
}