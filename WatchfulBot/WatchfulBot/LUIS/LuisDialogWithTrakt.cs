/*
 * Async tasks for each intent that's the the LUIS model.
 * Some link to trakt to get info about TV shows.
 * Last Modified: 22 Aug 2016.
 */ 
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WatchfulBot.Models;

namespace WatchfulBot.LUIS
{
    [LuisModel("b1cd889f-e37f-4694-bdb7-c19789b43205", "")]
    [Serializable]
    public class LuisDialogWithTrakt : LuisDialog<object>
    {
        public LuisDialogWithTrakt()
        {
        }

        public LuisDialogWithTrakt(ILuisService service) : base(service)
        {
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "Sorry that did not make sense. Please try again with another message";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("intent.watchfulbot.greetings.hello")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            string message = "Hey there! I'm WatchfulBot, you can ask for a list of popular/trending TV shows";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("intent.traktbotsession.movie.popular")]
        public async Task Popular(IDialogContext context, LuisResult result)
        {
            // Function 1:
            // Display the list of popular shows.
            var baseAddress = new Uri("https://api.trakt.tv/");
            string returnMessage = "The popular TV Shows are:" + Environment.NewLine;
            string path = null;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "38598ae35ea0cd9003624be5862b8ea9c294e5e68892e84cb2047b3a21d3277e");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");

                using (var responseMessage = httpClient.GetAsync(path).Result)
                {
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        // Bring the info into the model.
                        var responseJSON = JsonConvert.DeserializeObject<List<Shows_Popular>>(responseData);
                        // Make nice string.
                        foreach (var item in responseJSON)
                        {
                            returnMessage += string.Concat(Environment.NewLine, Environment.NewLine, item.shows.title);
                        }
                    }
                    else
                    {
                        returnMessage = "Did not work";
                    }

                }
            }

            // reply to user.
            await context.PostAsync(returnMessage);
            context.Wait(MessageReceived);
        }

        [LuisIntent("intent.traktbotsession.movie.trending")]
        public async Task Trending(IDialogContext context, LuisResult result)
        {
            // Function 2:
            // Display the list of trending shows.
            var baseAddress = new Uri("https://api.trakt.tv/");
            string returnMessage = "The trending TV Shows are:" +Environment.NewLine;
            string path = null;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-key", "38598ae35ea0cd9003624be5862b8ea9c294e5e68892e84cb2047b3a21d3277e");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("trakt-api-version", "2");
                
                using (var responseMessage = httpClient.GetAsync(path).Result)
                {
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                        // Bring the info into the model.
                        var responseJSON = JsonConvert.DeserializeObject<List<Shows_Trending>>(responseData);
                        // Make nice string.
                        foreach (var item in responseJSON)
                        {
                            returnMessage += string.Concat(Environment.NewLine, Environment.NewLine, item.shows.title);
                        }
                    }
                    else
                    {
                        returnMessage = "Did not work";
                    }

                }
            }
            // reply to user.
            await context.PostAsync(returnMessage);
            context.Wait(MessageReceived);
        }       
    }
}
