﻿namespace Praktika.Areas.Admin.ViewModels
{
    public class CreateServiceVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
