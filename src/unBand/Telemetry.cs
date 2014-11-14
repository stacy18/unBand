﻿using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unBand
{
    public static class Telemetry
    {

        public static TelemetryClient Client {get; private set;}

        static Telemetry()
        {
            Client = new TelemetryClient();
            Init();
        }

        private static void Init()
        {
            var props = Properties.Settings.Default;

            if (props.Device == Guid.Empty)
            {
                props.Device = Guid.NewGuid();
                props.Save();

                Client.Context.Session.IsFirst = true;
            }

            Client.Context.Device.Id = props.Device.ToString();
            Client.Context.Session.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Poor mans enum -> expanded string.
        /// 
        /// Once I've been using this for a while I may change this to a pure enum if 
        /// spaces in names prove to be annoying for querying / sorting the data
        /// </summary>
        public static class Events
        {
            public const string AppLaunch = "Launch";
            public const string DeclinedFirstRunWarning = "Declined First Run Warning";
            public const string DeclinedTelemetry = "Declined Telemetry";

            public const string ChangeBackground = "Change Background";
            public const string ChangeThemeColor = "Change Theme Color";
        }


    }
}