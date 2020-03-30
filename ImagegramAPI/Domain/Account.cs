using System.Collections.Generic;

namespace ImagegramAPI.Domain
{
    public class Account
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public Account()
        {

        }
        public Account(string name)
        {
            Name = name;
        }
    }
}
