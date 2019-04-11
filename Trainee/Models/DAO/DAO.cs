using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Trainee.Models.DAO
{
    public class DAO
    {
        db_Trainee db = new db_Trainee();
        public IEnumerable<Image> ListAllPaping(int page, int pageSize)
        {
            return db.Images.OrderByDescending(x => x.Idimg).ToPagedList(page, pageSize);
        }
    }
}