using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InputLibrary;
using Newtonsoft.Json;
using RestSharp;
using TicketingDB.Models;

namespace CounterManagement
{
    public class Program
    {
        private static RestClient client; 

        static void Main(string[] args)
        {
            client = new RestClient("http://localhost:54398/");
            bool end = false;
            bool counterEnd;
            do
            {
                displayMainMenu();
                int counterId = Input.getIntegerInput("Enter counter number", 1, 4);
                counterEnd = false;
                do
                {
                    Counter currCounter = displayCounterMenu(counterId);
                    int counterMenu = Input.getIntegerInput("Menu selection", 0, 3);

                    switch(counterMenu)
                    {
                        case 1:
                            updateCounterStatus(currCounter);
                            break;
                        case 2:
                            completeCurrent(currCounter);
                            break;
                        case 3:
                            callNext(currCounter);
                            break;
                        case 0:
                            counterEnd = true;
                            break;
                    }
                } while (!counterEnd);
            } while (!end);
        }

        private static void completeCurrent(Counter counter)
        {
            if(counter.Status.Equals("ONLINE"))
            {
                if (counter.CurrNum != 0)
                {
                    // update counter curr serving number
                    counter.CurrNum = 0;
                    RestRequest updateReq = new RestRequest("api/counters/{id}", Method.Put);
                    updateReq.AddUrlSegment("id", counter.Id);
                    updateReq.AddJsonBody(counter);
                    RestResponse updateResp = client.Execute(updateReq);
                }
                else
                {
                    Console.WriteLine("Counter is not currently serving any tickets");
                }
            }
            else
            {
                Console.WriteLine("Counter must be ONLINE to serve tickets");
            }
        }

        private static void callNext(Counter counter)
        {
            if(counter.Status.Equals("ONLINE"))
            {
                if (counter.CurrNum == 0)
                {
                    // get waiting queue
                    RestRequest request = new RestRequest("api/waitingqueue", Method.Get);
                    RestResponse response = client.Execute(request);
                    List<WaitingQueue> tickets = JsonConvert.DeserializeObject<List<WaitingQueue>>(response.Content);

                    if (tickets.Count > 0)
                    {
                        WaitingQueue nextTicket = tickets[0];

                        // update counter curr serving number
                        counter.CurrNum = nextTicket.TicketNum;
                        RestRequest updateReq = new RestRequest("api/counters/{id}", Method.Put);
                        updateReq.AddUrlSegment("id", counter.Id);
                        updateReq.AddJsonBody(counter);
                        RestResponse updateResp = client.Execute(updateReq);

                        // remove next ticket from waiting queue
                        RestRequest request2 = new RestRequest("api/waitingqueue/{num}", Method.Delete);
                        request2.AddUrlSegment("num", nextTicket.TicketNum);
                        RestResponse response2 = client.Execute(request2);

                        // update cur num in customer view
                        RestRequest getLatestServedReq = new RestRequest("api/latestserved", Method.Get);
                        RestResponse getLatestServedResp = client.Execute(getLatestServedReq);
                        List<LatestServed> list = JsonConvert.DeserializeObject<List<LatestServed>>(getLatestServedResp.Content);

                        if (list.Count == 2)
                        {
                            // remove the last number from latest served database
                            RestRequest removeLastNumReq = new RestRequest("api/latestserved/{num}", Method.Delete);
                            removeLastNumReq.AddUrlSegment("num", list[0].TicketNum);
                            RestResponse removeLastNumResp = client.Execute(removeLastNumReq);
                        }

                        // 
                        RestRequest request3 = new RestRequest("api/latestserved", Method.Post);
                        LatestServed latestServed = new LatestServed();
                        latestServed.TicketNum = nextTicket.TicketNum;
                        request3.AddJsonBody(latestServed);
                        RestResponse response3 = client.Execute(request3);
                    }
                    else
                    {
                        Console.WriteLine("No tickets in the waiting queue");
                    }
                }
                else
                {
                    Console.WriteLine("Please complete the current ticket first");
                }
            }
            else
            {
                Console.WriteLine("Counter must be ONLINE to serve tickets");
            }
            
            
        }

        private static void updateCounterStatus(Counter counter)
        {
            if(counter.Status.Equals("ONLINE"))
            {
                counter.Status = "OFFLINE";
            }
            else
            {
                counter.Status = "ONLINE";
            }

            RestRequest request = new RestRequest("api/counters/{id}", Method.Put);
            request.AddUrlSegment("id", counter.Id);
            request.AddJsonBody(counter);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                Console.WriteLine(String.Format("Counter {0} status updated", counter.Id));
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        private static void displayMainMenu()
        {
            string menu = "\nCounter Management\n\n";
            menu += "[1] Counter 1\n";
            menu += "[2] Counter 2\n";
            menu += "[3] Counter 3\n";
            menu += "[4] Counter 4\n";

            Console.Write(menu);
        }

        private static Counter displayCounterMenu(int counterId)
        {
            
            RestRequest request = new RestRequest("api/counters/{id}", Method.Get);
            request.AddUrlSegment("id", counterId);
            RestResponse response = client.Execute(request);
            Counter counter = JsonConvert.DeserializeObject<Counter>(response.Content);

            string menu = String.Format("\nCounter {0}\n", counter.Id);
            menu += "\n\tStatus: " + counter.Status + "\n";
            if(counter.Status.Equals("OFFLINE"))
            {
                menu += "\tCurrent Serving: OFFLINE" + "\n";
                menu += "\n\t[1] Go Online\n";
            }
            else
            {
                menu += "\tCurrent Serving: " + counter.CurrNum + "\n";
                menu += "\n\t[1] Go Offline\n";
            }

            menu += "\t[2] Complete Current\n";
            menu += "\t[3] Call Next\n";
            menu += "\n\t[0] Go Back\n";

            Console.Write(menu);
            return counter;
        }   
    }
}
