using System;

namespace Psychology_RequestResponse.Domain
{
    public class InterdepartResponse
    {
        public int Id { get; set; }
        public DateTime Request { get; set; }
        public int InterdepartStatusId { get; set; }
    }
}