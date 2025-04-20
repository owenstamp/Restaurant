using System;

namespace Restaurant.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; private set; }           // "FeedbackID"
        public Guid CustomerId { get; private set; }   // Link to Customers.CustomerID
        public Guid? OrderId { get; private set; }     // Nullable, if feedback is about an order
        public int Rating { get; private set; }        // 1-5
        public string Comments { get; private set; }
        public DateTime SubmittedAt { get; private set; }

        // Constructor
        public Feedback(Guid customerId, int rating, string comments, Guid? orderId = null)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Rating = rating;
            Comments = comments;
            OrderId = orderId;
            SubmittedAt = DateTime.UtcNow;
        }

        // Example domain logic
        public void UpdateRating(int newRating, string newComments)
        {
            Rating = newRating;
            Comments = newComments;
            // Possibly track an "updated at" field if you want
        }
    }
}
