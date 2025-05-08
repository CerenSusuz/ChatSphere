using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSphere.Domain.Entities;

    public class Message
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string SenderUsername { get; set; }

        public string RoomId { get; set; }

        public DateTime SentAt { get; set; }
    }

