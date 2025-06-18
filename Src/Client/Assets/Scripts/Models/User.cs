using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SkillBridge.Message;
using Services;

namespace Models
{
    class User : Singleton<User>
    {
        NUserInfo userInfo;

        public NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetupUserInfo(NUserInfo info)
        {
            this.userInfo = info;
        }

        public NCharacterInfo CurrentCharacter { get; set; }

        public MapDefine CurrentMap { get; set; }

        public PlayerInputController CurrentCharacterObject {  get; set; }

        public NTeamInfo TeamInfo { get; set; }

        public void AddGold(int gold)
        {
            this.CurrentCharacter.Gold += gold;
        }

        public int CurrentRide = 0;
        internal void Ride(int id)
        {
            if (CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject
            }
        }
    }
}