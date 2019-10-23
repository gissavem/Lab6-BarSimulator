﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        private Random random = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool isWorking = true; 
        //ITS ME, BLACKSMITH

        public Bouncer(Pub pub, LogHandler logHandler):base(pub, logHandler)
        {
        }
        public Patron LetPatronInside(Func<string> CheckID)
        {
            var patron = new Patron(Pub.Guests.Count, CheckID(), Pub, LogHandler);
            LogHandler.UpdateLog($" {patron.Name} joins the party.", LogHandler.MainWindow.GuestAndBouncerLog);
            return patron;
            
        }

        private string CheckID()
        {
            return NameList.AvailableNames.Take();
        }

        public override void GoHome()
        {
            cts.Cancel();
            isWorking = false;
            LogHandler.UpdateLog("The bouncer goes home.", LogHandler.MainWindow.GuestAndBouncerLog);
        }

        public override void Simulate()
        {
            var ct = cts.Token;
            var welcomeGuestsTask = Task.Run(() =>
            {
                while (ct.IsCancellationRequested == false)
                {
                    Thread.Sleep(random.Next(3000, 10000));
                    if (isWorking == false)
                    {
                        return;
                    }
                    Pub.Guests.TryAdd(Pub.Guests.Count, LetPatronInside(CheckID));
                }
            });
        }       
    }
}
