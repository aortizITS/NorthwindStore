﻿using Microsoft.AspNetCore.Mvc;
using Northwind.Store.Model;
using Northwind.Store.Web.Internet.Extensions;

namespace Northwind.Store.Web.Internet.Settings
{
    public class RequestSettings
    {
        readonly Controller _c;
        public RequestSettings(Controller c)
        {
            _c = c;
        }

        public Product ProductAdded
        {
            get
            {
                return _c.TempData.GetFromJson<Product>(nameof(ProductAdded)); ;
            }
            set
            {
                _c.TempData.SetAsJson(nameof(ProductAdded), value);
            }
        }
    }
}
