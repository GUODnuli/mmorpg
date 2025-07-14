using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{ 
    public class TeamService : Singleton<TeamService>, IDisposable
    {
        public void Init()
        {

        }

        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }



        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new()
            {
                Request = new NetMessageRequest()
            };
            message.Request.teamInviteReq.FromId = User.Instance.CurrentCharacterInfo.Id;
            message.Request.teamInviteReq.FromName = User.Instance.CurrentCharacterInfo.Name;
            message.Request.teamInviteReq.ToId = friendId;
            message.Request.teamInviteReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// �յ�����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInviteRequest(object sender, TeamInviteRequest message)
        {
            var confirm = MessageBox.Show(string.Format("{0} �����������顣", message.FromName), "�������", MessageBoxType.Confirm, "����", "�ܾ�");
            confirm.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, message);
            };
            confirm.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, message);
            };
        }

        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new()
            {
                Request = new NetMessageRequest()
            };
            message.Request.teamInviteRsp = new TeamInviteResponse();
            message.Request.teamInviteRsp.Result = accept ? Result.Success : Result.Failed;
            message.Request.teamInviteRsp.Errormsg = accept ? "��ӳɹ�" : "�Է��ܾ����������";
            message.Request.teamInviteRsp.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// �յ����������Ӧ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInviteResponse(object sender, TeamInviteResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.ToName + " �������Ķ���", "������ӳɹ�");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "�������ʧ��");
            }
        }

        private void OnTeamInfo(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnTeamInfo");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }

        public void SendTeamLeaveRequest()
        {
            Debug.Log("SendTeamLeaveRequest");
            NetMessage message = new()
            {
                Request = new NetMessageRequest()
            };
            message.Request.teamLeave = new TeamLeaveRequest();
            message.Request.teamLeave.TeamId = User.Instance.TeamInfo.Id;
            message.Request.teamLeave.characterId = User.Instance.CurrentCharacterInfo.Id;
            NetClient.Instance.SendMessage(message);
        }

        private void OnTeamLeave(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);
                MessageBox.Show("�˳��ɹ�", "�˳�����");
            }
            else
            {
                MessageBox.Show("�˳�ʧ��", "�˳�����", MessageBoxType.Error);
            }
        }
    }
}