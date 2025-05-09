using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPaymentRepository
    {
        Payment GetById(Guid id);
        void Add(Payment payment);
        void Update(Payment payment);
        IEnumerable<Payment> GetByOrderId(Guid orderId);
    }

    /// <summary>
    ///     
    /// </summary>
    public interface IPOSLogRepository
    {
        POSLog GetById(Guid id);
        void Add(POSLog log);
        IEnumerable<POSLog> GetByOrderId(Guid orderId);
    }
}
