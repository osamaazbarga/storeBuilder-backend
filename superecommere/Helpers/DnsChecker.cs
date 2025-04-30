using DnsClient;
using System.Net;

namespace superecommere.Helpers
{
    public class DnsChecker
    {

        private readonly LookupClient _dnsClient;

        public DnsChecker()
        {
            _dnsClient = new LookupClient();
        }

        public async Task<bool> HasVerificationRecordAsync(string domain, string expectedValue)
        {
            var result = await _dnsClient.QueryAsync(domain, QueryType.TXT);

            var txtRecords = result.Answers.TxtRecords();

            foreach (var record in txtRecords)
            {
                foreach (var txt in record.Text)
                {
                    if (txt.Contains(expectedValue))
                        return true;
                }
            }

            return false;
        }
        public static bool IsDomainPointingToServer(string domain, string serverIp)
        {
            try
            {
                var addresses = Dns.GetHostAddresses(domain);
                return addresses.Any(ip => ip.ToString() == serverIp);
            }
            catch { return false; }
        }
    }
}
