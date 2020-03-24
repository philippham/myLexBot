// <copyright file="IAWSLexService.cs" company="pnphi49@gmail.com">
// Copyright (c) pnphi49@gmail.com. All rights reserved.
// </copyright>

using Amazon.Lex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myLexBot.Services
{
    public interface IAWSLexService
    {
        Task<PostTextResponse> SendTextMessageToLexAsync(string messageToSend, string sessionId, Dictionary<string,string> lexSessionAttributes);
    }
}
