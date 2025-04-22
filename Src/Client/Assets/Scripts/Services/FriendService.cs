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
    class FriendService : Singleton<FriendService>, IDisposable
    {
        public UnityAction OnFriendUpdate;

        public void Init()
        {

        }

        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnfriendRemove);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnfriendRemove);
        }

        public void SendFriendAddRequest(int friendId, string friendName)
        {
            Debug.LogFormat("SendFriendAddRequest: Friend Id: {0}, Friend Name: {1}", friendId, friendName);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq = new FriendAddRequest
            {
                FromId = User.Instance.CurrentCharacter.Id,
                FromName = User.Instance.CurrentCharacter.Name,
                ToId = friendId,
                ToName = friendName
            };
            NetClient.Instance.SendMessage(message);
        }

        public void SendFriendAddResponse(bool accept, FriendAddRequest request)
        {
            Debug.LogFormat("SendFriendAddResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRsp = new FriendAddResponse
            {
                Result = accept ? Result.Success : Result.Failed,
                Errormsg = accept ? "�Է�ͬ������ĺ�������" : "�Է��ܾ�����ĺ�������",
                Request = request
            };
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// �յ���Ӻ�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        public void OnFriendAddRequest(object sender, FriendAddRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0}�������Ϊ����", request.FromName), "��������", MessageBoxType.Confirm, "����", "�ܾ�");
            confirm.OnYes = () =>
            {
                this.SendFriendAddResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendFriendAddResponse(false, request);
            };
        }

        /// <summary>
        /// �յ���Ӻ�����Ӧ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnFriendAddResponse(object sender, FriendAddResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.ToName + "�������������", "��Ӻ��ѳɹ�");
            }
            else
            {
                MessageBox.Show(message.Request.ToName + "�ܾ����������", "��Ӻ���ʧ��");
            }
        }

        private void OnFriendList(object sender, FriendListResponse message)
        {
            Debug.LogFormat("OnFriendList");
            FriendManager.Instance.allFriends = message.Friends;
            this.OnFriendUpdate?.Invoke();
        }

        public void SendFriendRemoveRequest(int friendId)
        {
            Debug.LogFormat("SendFriendRemoveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();
            message.Request.friendRemove.FriendId = friendId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnfriendRemove(object sender, FriendRemoveResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show("ɾ���ɹ�", "ɾ������");
            }
            else
            {
                MessageBox.Show("ɾ��ʧ��", "ɾ������", MessageBoxType.Error);
            }
        }
    }
}