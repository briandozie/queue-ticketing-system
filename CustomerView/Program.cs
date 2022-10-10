using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketingDB.Models;
using InputLibrary;

namespace CustomerView
{
    public class Program
    {
        private static RestClient client;
        private static int number = 0;
        static void Main(string[] args)
        {
            client = new RestClient("http://localhost:54398/");

            Timer timer = new Timer(RefreshView, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            while (true)
            {
                if(Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    /* This would have been the code to enable user to take a number through
                       the console */

                    // get the waiting queue
                    /*RestRequest getRequest = new RestRequest("api/waitingqueue", Method.Get);
                    RestResponse getResponse = client.Execute(getRequest);
                    List<WaitingQueue> waitingQueue = JsonConvert.DeserializeObject<List<WaitingQueue>>(getResponse.Content);

                    // add new ticket to waiting queue
                    RestRequest request = new RestRequest("api/waitingqueue", Method.Post);
                    WaitingQueue ticket = new WaitingQueue();
                    ticket.TicketNum = waitingQueue.Count + 1;
                    request.AddJsonBody(request);
                    RestResponse response = client.Execute(request);

                    number = ticket.TicketNum;*/
                }
            }
        }

        private static void RefreshView(Object o)
        {
            Console.Clear();
            displayMenu();
        }

        private static void displayMenu()
        {
            string menu = "\nCustomer View\n";
            menu += displayNowServing();
            menu += displayCounterInformation(1);
            menu += displayCounterInformation(2);
            menu += displayCounterInformation(3);
            menu += displayCounterInformation(4);
            menu += displayNumber();

            Console.Write(menu);
        }

        private static string displayNumber()
        {
            string returnStr = "\n";
            
            if(number == 0)
            {
                returnStr += "Your number is: \n";
            }
            else
            {
                returnStr += "Your number is: " + number + "\n";
            }
            returnStr += "Press enter to take a number ";
            return returnStr;
        }

        private static string displayNowServing()
        {
            RestRequest nowServingReq = new RestRequest("api/latestserved", Method.Get);
            RestResponse nowServingResp = client.Execute(nowServingReq);
            List<LatestServed> latestServed = JsonConvert.DeserializeObject<List<LatestServed>>(nowServingResp.Content);
            string returnStr = "\n";

            if(latestServed.Count == 0)
            {
                returnStr += "Now Serving: -\n";
                returnStr += "Last Number: -\n";
            }
            else if(latestServed.Count == 1)
            {
                returnStr += "Now Serving: " + latestServed[0].TicketNum + "\n";
                returnStr += "Last Number: -\n";
            }
            else
            {
                returnStr += "Now Serving: " + latestServed[1].TicketNum + "\n";
                returnStr += "Last Number: " + latestServed[0].TicketNum + "\n";
            }

            return returnStr;
        }

        private static string displayCounterInformation(int counterId)
        {
            string returnStr = "\nCounter " + counterId + "\n";

            RestRequest request = new RestRequest("api/counters/{id}", Method.Get);
            request.AddUrlSegment("id", counterId);
            RestResponse response = client.Execute(request);
            Counter counter = JsonConvert.DeserializeObject<Counter>(response.Content);

            if (counter.Status.Equals("OFFLINE"))
            {
                returnStr += "\tStatus: OFFLINE\n";
                returnStr += "\tCurrent Number: OFFLINE\n";
            }
            else if (counter.CurrNum == 0)
            {
                returnStr += "\tStatus: ONLINE\n";
                returnStr += "\tCurrent Number: -\n";
            }
            else
            {
                returnStr += "\tStatus: BUSY\n";
                returnStr += "\tCurrent Number: " + counter.CurrNum + "\n";
            }

            return returnStr;
        }
    }

    
    
}
