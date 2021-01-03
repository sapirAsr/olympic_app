using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
  
namespace olympic_app.Models
{
    public class Question
    {
        public string QuestionString { get; set; }
  
        public string CorrectAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string WrongAnswer3 { get; set; }
        public string Sport { get; set; }
    }
}