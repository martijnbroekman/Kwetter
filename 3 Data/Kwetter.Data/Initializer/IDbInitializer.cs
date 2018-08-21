using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Data.Initializer
{
    public interface IDbInitializer
    {
        void Initialize(IApplicationBuilder app);
    }
}
