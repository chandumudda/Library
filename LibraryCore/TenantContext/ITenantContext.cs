﻿
namespace LibraryCore.TenantContext
{
    public interface ITenantContext
    {
        string User { get; }
        string AccessToken { get; }
    }
}
