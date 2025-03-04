using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Identity;

namespace Bulky.DataAccess.DbInitializer;

public interface IDbInitializer
{
    public void Initialize();
}