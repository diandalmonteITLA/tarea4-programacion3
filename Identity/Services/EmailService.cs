using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;

namespace Identity.Services
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(EmailRequestDto emailRequestDto)
        {
            return Task.CompletedTask;
        }
    }
}
