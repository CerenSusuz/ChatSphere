namespace ChatSphere.Domain.Entities
{
    public class ChatUserRoom
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}
