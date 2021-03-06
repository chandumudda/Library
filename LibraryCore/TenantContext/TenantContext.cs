﻿
using Microsoft.AspNetCore.Http;
using System;

namespace LibraryCore.TenantContext
{
    public class TenantContext : ITenantContext
    {
        private readonly HttpContext _httpContext;

        public TenantContext(
            IHttpContextAccessor httpContentAccessor)
        {
            _httpContext = httpContentAccessor.HttpContext;
        }

        public string User
        {
            get
            {
                ValidateHttpContext();
                return _httpContext.User.Identity.Name;
            }
        }

        public string AccessToken
        {
            get
            {
                ValidateHttpContext();
                return _httpContext.Request.Headers["Authorization"];
            }
        }

        private void ValidateHttpContext()
        {
            if (_httpContext == null)
                throw new ArgumentNullException(nameof(_httpContext));
        }
    }
}