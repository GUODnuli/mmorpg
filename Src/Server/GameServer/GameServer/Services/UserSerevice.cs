﻿using System;
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

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();
            
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "该用户未注册，请注册后重新登陆.";
            }
            else if (user.Password != request.Password)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "密码错误！";
            }
            else
            {
                sender.Session.User = user;

                message.Response.userLogin.Result = Result.Success;
                message.Response.userLogin.Errormsg = "登陆成功！";
                message.Response.userLogin.Userinfo = new NUserInfo();
                message.Response.userLogin.Userinfo.Id = (int)user.ID;
                message.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                message.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach(var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    info.Tid = c.ID;
                    info.Type = CharacterType.Player;
                    message.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User: {0}, Password: {1}", request.User, request.Password);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Password, Player = player });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "创建成功！";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacter: Name: {0}, CharacterClass: {1}", request.Name, request.Class);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.createChar = new UserCreateCharacterResponse();

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
                        Tid = character.ID,
                        Type = CharacterType.Player,
                    };
                    message.Response.createChar.Characters.Add(nCharacterInfo);
                }
                message.Response.createChar.Result = Result.Success;
                message.Response.createChar.Errormsg = "创建成功！";
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("保存新角色失败: {0}", ex.Message);
                message.Response.createChar.Result = Result.Failed;
                message.Response.createChar.Errormsg = "创建失败！";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: CharacterID: {0}, CharacterName: {1}, Map: {0}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameEnter = new UserGameEnterResponse();
            message.Response.gameEnter.Result = Result.Success;
            message.Response.gameEnter.Errormsg = "成功进入游戏世界！";
            message.Response.gameEnter.Character = character.Info;

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
            sender.Session.Character = character;
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.gameLeave = new UserGameLeaveResponse();
            try
            {
                Character character = sender.Session.Character;
                Log.InfoFormat("UserGameLeaveRequest: characterID: {0}, CharacterName: {1}, MapID: {2}", character.Id, character.Info.Name, character.Info.mapId);
                UserLeaveGame(character);

                message.Response.gameLeave.Result = Result.Success;
                message.Response.gameLeave.Errormsg = "离开游戏成功！";
            }
            catch (Exception e)
            {
                message.Response.gameLeave.Result = Result.Failed;
                message.Response.gameLeave.Errormsg = string.Format("离开游戏失败！失败原因：{0}", e);
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        public void UserLeaveGame(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.Id);
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }
}