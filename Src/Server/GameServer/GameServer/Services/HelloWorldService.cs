using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class HelloWorldService
    {
        private static HelloWorldService instance = null;
        private bool isInitialized = false;

        private HelloWorldService() { }

        public static HelloWorldService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HelloWorldService();
                }
                return instance; 
            }
        }

        public void Init()
        {
            if (!isInitialized)
            {
                isInitialized = true;
            }
        }

        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }

        void OnFirstTestRequest(NetConnection<NetSession> sender, FirstTestRequest request)
        {
            Log.InfoFormat("OnFirstTestRequest: Helloworld: {0}", request.Helloworld);
        }
    }
}
