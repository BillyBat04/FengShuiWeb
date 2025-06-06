﻿namespace FengShuiWeb.Infrastructure
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
        public bool UseTLS { get; set; }
        public string FromName { get; set; }
    }
}