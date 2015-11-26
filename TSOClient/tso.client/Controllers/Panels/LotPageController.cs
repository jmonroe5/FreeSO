﻿using FSO.Client.Network;
using FSO.Client.UI.Panels;
using FSO.Common.DataService;
using FSO.Common.DataService.Model;
using FSO.Server.DataService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSO.Client.Controllers.Panels
{
    public class LotPageController
    {
        private UILotPage View;
        private IClientDataService DataService;
        private ITopicSubscription Topic;
        private uint LotId;

        public LotPageController(UILotPage view, IClientDataService dataService)
        {
            this.View = view;
            this.DataService = dataService;
            this.Topic = dataService.CreateTopicSubscription();
        }

        ~LotPageController(){
            Topic.Dispose();
        }

        public void Close()
        {
            View.Visible = false;
            ChangeTopic();
        }

        public void Show(uint lotId)
        {
            LotId = lotId;
            DataService.Get<Lot>(lotId).ContinueWith(x =>
            {
                View.CurrentLot.Value = x.Result;
            });
            View.Visible = true;
            ChangeTopic();
        }

        private void ChangeTopic()
        {
            List<ITopic> topics = new List<ITopic>();
            if (View.Visible && LotId != 0)
            {
                topics.Add(Topics.For(MaskedStruct.PropertyPage_LotInfo, LotId));
            }
            Topic.Set(topics);
        }
    }
}