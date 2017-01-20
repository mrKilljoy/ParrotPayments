using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ParrotPaymentsApp.Models;

namespace ParrotPaymentsApp.Infrastructure
{
    public class PaymentsManager : IDisposable
    {
        private AuthContext _cnt;

        public PaymentsManager()
        {
            _cnt = new AuthContext();
        }

        public void Dispose()
        {
            ((IDisposable)_cnt).Dispose();
        }

        /// <summary>
        /// Выполнить перевод средств.
        /// </summary>
        /// <param name="sender_login">Логин отправителя.</param>
        /// <param name="receiver_login">Логин получателя.</param>
        /// <param name="amount">Сумма перевода.</param>
        /// <returns></returns>
        public PaymentOperationResult CreateNewPayment(string sender_login, string receiver_login, int amount)
        {
            using (var transaction = _cnt.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(sender_login) || string.IsNullOrWhiteSpace(receiver_login))
                        throw new ArgumentException("Один из участников операции не указан!");

                    if (amount < 1)
                        throw new ArgumentException("Сумма перевода не может быть ниже 1 Р!");
                    
                    ParrotUser receiver = _cnt.Users.FirstOrDefault(p => p.UserName == receiver_login);
                    ParrotUser sender = _cnt.Users.FirstOrDefault(p => p.UserName == sender_login);

                    if (receiver == null)
                        throw new Exception("Получатель перевода не обнаружен!");

                    receiver.AccountBalance += amount;
                    sender.AccountBalance -= amount;

                    if (sender.AccountBalance < 0)
                        throw new InvalidOperationException("Недостаточно средств для отправки!");

                    PaymentOperation op = new PaymentOperation()
                    {
                        SenderId = sender.Id,
                        SenderUsername = sender.UserName,
                        CorrespondentId = receiver.Id,
                        CorrespondentUsername = receiver.UserName,
                        Amount = amount,
                        Date = DateTime.Now,
                        SenderPostBalance = sender.AccountBalance,
                        CorrespondentPostBalance = receiver.AccountBalance
                    };

                    _cnt.Payments.Add(op);

                    _cnt.SaveChanges();
                    transaction.Commit();

                    return new PaymentOperationResult { Succeeded = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new PaymentOperationResult { Succeeded = false, LastException = ex };
                }
            }
        }

        /// <summary>
        /// Получить историю переводов пользователя.
        /// </summary>
        /// <param name="user_login">Логин пользователя.</param>
        /// <param name="payment_type">Тип перевода (0 - все, 1 - входящие, 2 - исходящие).</param>
        /// <param name="count">Число получаемых записей.</param>
        /// <returns></returns>
        public PaymentVM[] GetPaymentsHistory(string user_login, int payment_type, int count)
        {
            PaymentRelationType p_type = (PaymentRelationType)payment_type;

            try
            {
                var user = _cnt.Users.FirstOrDefault(u => u.UserName == user_login);
                if (user == null)
                    throw new Exception("Пользователь не найден!");

                IEnumerable<PaymentOperation> found = null;

                switch (p_type)
                {
                    case PaymentRelationType.Outgoing:
                        {
                            found = user.PaymentsSend.Take(count).ToArray();
                            break;
                        }
                    case PaymentRelationType.Ingoing:
                        {
                            found = user.PaymentsReceived.Take(count).ToArray();
                            break;
                        }
                    case PaymentRelationType.All:
                        {
                            var found_inbound = user.PaymentsReceived.Take(count).ToArray();
                            var found_outbound = user.PaymentsSend.Take(count).ToArray();

                            found = found_inbound.Union(found_outbound);
                            break;
                        }
                }

                List<PaymentVM> buffer_list = new List<PaymentVM>();

                foreach (var item in found)
                {
                    var vm = PaymentVMConverter.ConvertToViewModel(item);
                    buffer_list.Add(vm);
                }

                return buffer_list.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public enum PaymentRelationType
        {
            All = 0,
            Ingoing = 1,
            Outgoing = 2
        }
    }
}