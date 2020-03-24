// <copyright file="AWSLexService.cs" company="pnphi49@gmail.com">
// Copyright (c) pnphi49@gmail.com. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lex;
using Amazon.Lex.Model;
using Microsoft.Extensions.Logging;
using myLexBot.Options;
using Microsoft.Extensions.Options;

namespace myLexBot.Services
{
    public class AWSLexService : IAWSLexService
    {
        private readonly AmazonLexClient _awsLexClient;
        private readonly ILogger<AWSLexService> _logger;
        private readonly LexOptions _lexOptions;

        public AWSLexService(AmazonLexClient awsLexClient, ILogger<AWSLexService> logger, IOptions<LexOptions> lexOptions)
        {
            _awsLexClient = awsLexClient;
            _logger = logger;
            _lexOptions = lexOptions.Value;
        }

        public async Task<PostTextResponse> SendTextMessageToLexAsync(string messageToSend, string sessionId, Dictionary<string, string> lexSessionAttributes)
        {
            PostTextResponse lextTextResponse;
            var lexTextRequest = new PostTextRequest
            {
                BotAlias = _lexOptions.LexBotAlias,
                BotName = _lexOptions.LexBotName,
                UserId = sessionId,
                InputText = messageToSend,
                SessionAttributes = lexSessionAttributes
            };

            try
            {
                lextTextResponse = await _awsLexClient.PostTextAsync(lexTextRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AWSLexService.SendTextMessageToLexAsync got an exception for a request: " +
                    $"{Newtonsoft.Json.JsonConvert.SerializeObject(lexTextRequest)} with message: {ex.Message}");
                throw;
            }

            return lextTextResponse;
        }
    }
}
