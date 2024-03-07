using InitialProject.Domain.Model;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Injector;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public class VoucherService
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherService()
        {
            _voucherRepository = Inject.CreateInstance<IVoucherRepository>();
        }

        public void Delete(Voucher voucher)
        {
            _voucherRepository.Delete(voucher);
        }

        public Voucher Save(Voucher voucher)
        {
            return _voucherRepository.Save(voucher);
        }

        public List<Voucher> GetUpcomingVouchers(User user)
        {
            List<Voucher> Vouchers = new List<Voucher>();
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            foreach (Voucher voucher in _voucherRepository.GetAll())
            {
                AddValidVouchers(user, Vouchers, today, voucher);
            }
            return Vouchers;
        }

        private static void AddValidVouchers(User user, List<Voucher> Vouchers, DateOnly today, Voucher voucher)
        {
            if (voucher.IdUser==user.Id && voucher.ExpirationDate.CompareTo(today)>0)
            {
                Vouchers.Add(voucher);
            }
        }

        public List<Voucher> GetAll()
        {
            return _voucherRepository.GetAll();
        }


    }
}
