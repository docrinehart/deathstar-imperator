﻿using System;
using DeathStarImperator.Core;
using Microsoft.AspNet.SignalR;

namespace DeathStarImperator.UI.Hubs
{
    public class AlertHub : Hub, IAlertHub
    {
        public void CreateAlert(string message)
        {
            var centralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            Clients.All.addNewAlertToPage(centralTime.ToString("HH:mm:ss.fff"), message);
        }
    }
}