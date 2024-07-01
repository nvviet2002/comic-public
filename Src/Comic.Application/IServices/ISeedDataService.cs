using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface ISeedDataService
    {
        Task InitSeedDataAsync();
    }
}
