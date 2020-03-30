using System.Collections.Generic;

namespace ImagegramAPI.Domain
{
    public class Account
    {
        public long Id { get; private set; }
        
        public string Name { get; private set; }

        public virtual List<Post> Posts { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public Account(string name)
        {
            Name = name;
        }
    }
}
