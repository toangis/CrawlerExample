using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerExample.Models
{
    public class Question
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public int Votes { get; set; }

        public int Views { get; set; }

        public int Answers { get; set; }

    }
}
