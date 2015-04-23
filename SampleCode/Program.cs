using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NabVelocity.Svc;
using NabVelocity.Txn;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Preparing the Application to Transact

            #region Setup Clients

            // setup service information client from service reference generated code
            var svcClient = new CWSServiceInformationClient();

            // setup transaction client from service reference generated code
            var txnClient = new CwsTransactionProcessingClient(new BasicHttpsBinding(),
                new EndpointAddress("https://api.cert.nabcommerce.com/2.0.18/Txn"));

            #endregion

            #region SignOnWithToken

            // test identity token for global host capture
            string identityToken = "PHNhbWw6QXNzZXJ0aW9uIE1ham9yVmVyc2lvbj0iMSIgTWlub3JWZXJzaW9uPSIxIiBBc3NlcnRpb25JRD0iXzhhYTA2NDAxLTFlYzAtNDg1ZS1hYzNjLTRjYjEwZTdkNTE1MyIgSXNzdWVyPSJJcGNBdXRoZW50aWNhdGlvbiIgSXNzdWVJbnN0YW50PSIyMDE0LTA4LTAxVDE4OjQ3OjEyLjkyN1oiIHhtbG5zOnNhbWw9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjEuMDphc3NlcnRpb24iPjxzYW1sOkNvbmRpdGlvbnMgTm90QmVmb3JlPSIyMDE0LTA4LTAxVDE4OjQ3OjEyLjkyN1oiIE5vdE9uT3JBZnRlcj0iMjAxNy0wOC0wMVQxODo0NzoxMi45MjdaIj48L3NhbWw6Q29uZGl0aW9ucz48c2FtbDpBZHZpY2U+PC9zYW1sOkFkdmljZT48c2FtbDpBdHRyaWJ1dGVTdGF0ZW1lbnQ+PHNhbWw6U3ViamVjdD48c2FtbDpOYW1lSWRlbnRpZmllcj4yM0EzMzY3OTE2NjAwMDAxPC9zYW1sOk5hbWVJZGVudGlmaWVyPjwvc2FtbDpTdWJqZWN0PjxzYW1sOkF0dHJpYnV0ZSBBdHRyaWJ1dGVOYW1lPSJTQUsiIEF0dHJpYnV0ZU5hbWVzcGFjZT0iaHR0cDovL3NjaGVtYXMuaXBjb21tZXJjZS5jb20vSWRlbnRpdHkiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlPjIzQTMzNjc5MTY2MDAwMDE8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgQXR0cmlidXRlTmFtZT0iU2VyaWFsIiBBdHRyaWJ1dGVOYW1lc3BhY2U9Imh0dHA6Ly9zY2hlbWFzLmlwY29tbWVyY2UuY29tL0lkZW50aXR5Ij48c2FtbDpBdHRyaWJ1dGVWYWx1ZT5mYTEzZjVmNS04N2IyLTQ5YzctOGIyZS1iOGE2NTY2ZTNlY2U8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgQXR0cmlidXRlTmFtZT0ibmFtZSIgQXR0cmlidXRlTmFtZXNwYWNlPSJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcyI+PHNhbWw6QXR0cmlidXRlVmFsdWU+MjNBMzM2NzkxNjYwMDAwMTwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjwvc2FtbDpBdHRyaWJ1dGVTdGF0ZW1lbnQ+PFNpZ25hdHVyZSB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyI+PFNpZ25lZEluZm8+PENhbm9uaWNhbGl6YXRpb25NZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiPjwvQ2Fub25pY2FsaXphdGlvbk1ldGhvZD48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI3JzYS1zaGExIj48L1NpZ25hdHVyZU1ldGhvZD48UmVmZXJlbmNlIFVSST0iI184YWEwNjQwMS0xZWMwLTQ4NWUtYWMzYy00Y2IxMGU3ZDUxNTMiPjxUcmFuc2Zvcm1zPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSI+PC9UcmFuc2Zvcm0+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMTAveG1sLWV4Yy1jMTRuIyI+PC9UcmFuc2Zvcm0+PC9UcmFuc2Zvcm1zPjxEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjc2hhMSI+PC9EaWdlc3RNZXRob2Q+PERpZ2VzdFZhbHVlPjVvTTlwTjdQYlpIYjBVRDVmV0tVb0RmemN3RT08L0RpZ2VzdFZhbHVlPjwvUmVmZXJlbmNlPjwvU2lnbmVkSW5mbz48U2lnbmF0dXJlVmFsdWU+S0grYlJhc04zQmFUeVFhaEE5UDFJQWtXVjg3bG84RDBibXNvZVhzeEU1Z3NjZHVOczFDQ25qVXowOHBkSE1TbXlpODJTQVJYcmZoQStWTHF2UkZoM3hPcTYyc1ltWGtET0NzUGJ3dnRIUGwrUmV4NEdPc3dKSDlyR3pDbU51NVZ2U0VYTEIva2NuaG1EU1VuTTl1RE4wWXE5dXIxZG1NZTZMZzBxOExzNCtyaDJvbEk0QW0reDRPTEdOVVlpRXAvSzFQTWw3TGtOSmo1aVA3dTFVeE4rUVhQbDlsRWJucExiWExGRkJJUktER1dmaDJZcG9ZM0JFbXI0TXRMYUNoUW13bnVJTDlNbmozRGdYMVQ3RHJiMFBBS1FRV211bGFMZG41a1pCTXJ3eENwWk1tR0JCOFExMnAzQVhYZjVZOGxHNUtJL1djQnNFeG95YkFUTm1ZenlnPT08L1NpZ25hdHVyZVZhbHVlPjxLZXlJbmZvPjxvOlNlY3VyaXR5VG9rZW5SZWZlcmVuY2UgeG1sbnM6bz0iaHR0cDovL2RvY3Mub2FzaXMtb3Blbi5vcmcvd3NzLzIwMDQvMDEvb2FzaXMtMjAwNDAxLXdzcy13c3NlY3VyaXR5LXNlY2V4dC0xLjAueHNkIj48bzpLZXlJZGVudGlmaWVyIFZhbHVlVHlwZT0iaHR0cDovL2RvY3Mub2FzaXMtb3Blbi5vcmcvd3NzL29hc2lzLXdzcy1zb2FwLW1lc3NhZ2Utc2VjdXJpdHktMS4xI1RodW1icHJpbnRTSEExIj5ZREJlRFNGM0Z4R2dmd3pSLzBwck11OTZoQ2M9PC9vOktleUlkZW50aWZpZXI+PC9vOlNlY3VyaXR5VG9rZW5SZWZlcmVuY2U+PC9LZXlJbmZvPjwvU2lnbmF0dXJlPjwvc2FtbDpBc3NlcnRpb24+";
            // test identity token for global term capture
            //string identityToken = "PHNhbWw6QXNzZXJ0aW9uIE1ham9yVmVyc2lvbj0iMSIgTWlub3JWZXJzaW9uPSIxIiBBc3NlcnRpb25JRD0iX2ZiNWZjOWM4LTM3YTUtNDQ0MC05Mzc5LWU4OGE0ZDA5ZWI3OSIgSXNzdWVyPSJJcGNBdXRoZW50aWNhdGlvbiIgSXNzdWVJbnN0YW50PSIyMDE0LTA4LTAxVDIzOjQ3OjQwLjA3NVoiIHhtbG5zOnNhbWw9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjEuMDphc3NlcnRpb24iPjxzYW1sOkNvbmRpdGlvbnMgTm90QmVmb3JlPSIyMDE0LTA4LTAxVDIzOjQ3OjQwLjA3NVoiIE5vdE9uT3JBZnRlcj0iMjAxNy0wOC0wMVQyMzo0Nzo0MC4wNzVaIj48L3NhbWw6Q29uZGl0aW9ucz48c2FtbDpBZHZpY2U+PC9zYW1sOkFkdmljZT48c2FtbDpBdHRyaWJ1dGVTdGF0ZW1lbnQ+PHNhbWw6U3ViamVjdD48c2FtbDpOYW1lSWRlbnRpZmllcj4zQjhGQjVEQTA4MTAwMDAxPC9zYW1sOk5hbWVJZGVudGlmaWVyPjwvc2FtbDpTdWJqZWN0PjxzYW1sOkF0dHJpYnV0ZSBBdHRyaWJ1dGVOYW1lPSJTQUsiIEF0dHJpYnV0ZU5hbWVzcGFjZT0iaHR0cDovL3NjaGVtYXMuaXBjb21tZXJjZS5jb20vSWRlbnRpdHkiPjxzYW1sOkF0dHJpYnV0ZVZhbHVlPjNCOEZCNURBMDgxMDAwMDE8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgQXR0cmlidXRlTmFtZT0iU2VyaWFsIiBBdHRyaWJ1dGVOYW1lc3BhY2U9Imh0dHA6Ly9zY2hlbWFzLmlwY29tbWVyY2UuY29tL0lkZW50aXR5Ij48c2FtbDpBdHRyaWJ1dGVWYWx1ZT5kOGY5ZTNhYS1kMzI1LTQ4ODItYTIwMi1lNzBlNmRiMmZjZGI8L3NhbWw6QXR0cmlidXRlVmFsdWU+PC9zYW1sOkF0dHJpYnV0ZT48c2FtbDpBdHRyaWJ1dGUgQXR0cmlidXRlTmFtZT0ibmFtZSIgQXR0cmlidXRlTmFtZXNwYWNlPSJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcyI+PHNhbWw6QXR0cmlidXRlVmFsdWU+M0I4RkI1REEwODEwMDAwMTwvc2FtbDpBdHRyaWJ1dGVWYWx1ZT48L3NhbWw6QXR0cmlidXRlPjwvc2FtbDpBdHRyaWJ1dGVTdGF0ZW1lbnQ+PFNpZ25hdHVyZSB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyI+PFNpZ25lZEluZm8+PENhbm9uaWNhbGl6YXRpb25NZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiPjwvQ2Fub25pY2FsaXphdGlvbk1ldGhvZD48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI3JzYS1zaGExIj48L1NpZ25hdHVyZU1ldGhvZD48UmVmZXJlbmNlIFVSST0iI19mYjVmYzljOC0zN2E1LTQ0NDAtOTM3OS1lODhhNGQwOWViNzkiPjxUcmFuc2Zvcm1zPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSI+PC9UcmFuc2Zvcm0+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMTAveG1sLWV4Yy1jMTRuIyI+PC9UcmFuc2Zvcm0+PC9UcmFuc2Zvcm1zPjxEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjc2hhMSI+PC9EaWdlc3RNZXRob2Q+PERpZ2VzdFZhbHVlPkE3dCsrWExYMXpZQzVzMHhoSlZnR3ZXRUV4ST08L0RpZ2VzdFZhbHVlPjwvUmVmZXJlbmNlPjwvU2lnbmVkSW5mbz48U2lnbmF0dXJlVmFsdWU+aFMvWU8zelloVk1lQ3JRZU1PZVFFVnZrMVdVTjZCbzY1dGpmQVlIeVArZE1tRkVKa0hnSW1qa1BKTHN6ZU1HYjVNZmU3U2dWcWt6MmkvTGpzNFp4ZFlpL3VFTGY1L3pNbE44OXQ3MjR1cEtCRXhJdFZMc0R1eFVTY3Zob1VqcGZWLzFyZjQrR0YzQjJWNkp2clhjaTlTaDB5eHFkTXY1WUNDMlZmSEJxMkx1bHovd0tvSi9na3FmRnd5MEJHS05oMUxpVEJEcVV6SkYxcWs2U25zYXdkVE5VaEpsU2V0QzBvNmcrd3FUc0pZRHBLM1pFNGdyaWVoaXZ1U0FaTnpLSzlxSHJxeUtYOGtWT1Mvei80cEI2QTR5d0xCTDJMYTgvYW45bzFUcmFnZ1NrREJURURCTW85WExLMmUyQlVZNExwM2VaYkpaMzV2bU9VaHZ5OVNMV0hBPT08L1NpZ25hdHVyZVZhbHVlPjxLZXlJbmZvPjxvOlNlY3VyaXR5VG9rZW5SZWZlcmVuY2UgeG1sbnM6bz0iaHR0cDovL2RvY3Mub2FzaXMtb3Blbi5vcmcvd3NzLzIwMDQvMDEvb2FzaXMtMjAwNDAxLXdzcy13c3NlY3VyaXR5LXNlY2V4dC0xLjAueHNkIj48bzpLZXlJZGVudGlmaWVyIFZhbHVlVHlwZT0iaHR0cDovL2RvY3Mub2FzaXMtb3Blbi5vcmcvd3NzL29hc2lzLXdzcy1zb2FwLW1lc3NhZ2Utc2VjdXJpdHktMS4xI1RodW1icHJpbnRTSEExIj5ZREJlRFNGM0Z4R2dmd3pSLzBwck11OTZoQ2M9PC9vOktleUlkZW50aWZpZXI+PC9vOlNlY3VyaXR5VG9rZW5SZWZlcmVuY2U+PC9LZXlJbmZvPjwvU2lnbmF0dXJlPjwvc2FtbDpBc3NlcnRpb24+";

            string sessionToken = svcClient.SignOnWithToken(identityToken);

            #endregion

            #region SaveApplicationData

            var applicationData = new ApplicationData()
            {
                ApplicationAttended = false,
                ApplicationLocation = NabVelocity.Svc.ApplicationLocation.OffPremises,
                ApplicationName = "MyTestApp",
                EncryptionType = NabVelocity.Svc.EncryptionType.NotSet,
                HardwareType = NabVelocity.Svc.HardwareType.PC,
                PINCapability = NabVelocity.Svc.PINCapability.PINNotSupported,
                PTLSSocketId = "MIIFCzCCA/OgAwIBAgICAoEwDQYJKoZIhvcNAQEFBQAwgbExNDAyBgNVBAMTK0lQIFBheW1lbnRzIEZyYW1ld29yayBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkxCzAJBgNVBAYTAlVTMREwDwYDVQQIEwhDb2xvcmFkbzEPMA0GA1UEBxMGRGVudmVyMRowGAYDVQQKExFJUCBDb21tZXJjZSwgSW5jLjEsMCoGCSqGSIb3DQEJARYdYWRtaW5AaXBwYXltZW50c2ZyYW1ld29yay5jb20wHhcNMTMwODI2MTcxMDI3WhcNMjMwODI0MTcxMDI3WjCBjDELMAkGA1UEBhMCVVMxETAPBgNVBAgTCENvbG9yYWRvMQ8wDQYDVQQHEwZEZW52ZXIxGjAYBgNVBAoTEUlQIENvbW1lcmNlLCBJbmMuMT0wOwYDVQQDEzR0ZHNwM25TZ0FJQUFBUDhBSCtDWUFBQUVBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUE9MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtn6ILI78EaOLcWrmI9RZf8Vj+3P/WcrDLimSyJJH/8LnIBbXNkiKcZSMg/KHqNLAtq/ncYqZcicgAfaoSbj9FxKGIXTDEICriv/i8sQIGFhIwW/V6H02E8SpWjdCQO9EUUaFPUVMhHfiabwJ3B0VODsQfVuG7mbrAvD/wAqiUVR2Q0rpgHkToCkytdhMlkXiFtnfy4nnoFnI6c5cmsQU7AZgI6Zr08pDMN9y3uSRGSJIzdcTohBA1qb8C4+ZVRCmwCfQZiBHxjC8c5DTiGlPQVEDfRjKXm6ffqBKCttX7qCeB0s57iob0Q7ucz8NfoWtY8dZVzMhYH8obU/dSXaZ6wIDAQABo4IBTjCCAUowCQYDVR0TBAIwADAdBgNVHQ4EFgQUJ64+T3k9d5nWfplPlxVZsN382XUwgeYGA1UdIwSB3jCB24AU3+ASnJQimuunAZqQDgNcnO2HuHShgbekgbQwgbExNDAyBgNVBAMTK0lQIFBheW1lbnRzIEZyYW1ld29yayBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkxCzAJBgNVBAYTAlVTMREwDwYDVQQIEwhDb2xvcmFkbzEPMA0GA1UEBxMGRGVudmVyMRowGAYDVQQKExFJUCBDb21tZXJjZSwgSW5jLjEsMCoGCSqGSIb3DQEJARYdYWRtaW5AaXBwYXltZW50c2ZyYW1ld29yay5jb22CCQD/yDY5hYVsVzA1BgNVHR8ELjAsMCqgKKAmhiRodHRwOi8vY3JsLmlwY29tbWVyY2UuY29tL2NhLWNybC5jcmwwDQYJKoZIhvcNAQEFBQADggEBAJrku2QD0T/0aT+jfFJA947Vf7Vu/6S1OxUGhMipx6z/izXZ+o4fK/Nsg0G39KvfxippFG/3MUo621dwXwtqq9SM72zy9ry9E0ptmEiG8X8bSVOyGj4MqyExCPs9LgloV5GgewqYRgq2hmbXOv8Gw7EeXGCfnQ+eROxGu1+p3ZWUnGMQnBbayg43npcHYfyLFHOzd57pj6ncYoxY3kun5GLMLr6tJXKpPNvbM5lAOzcAmKviPMCM2T53UzJlsRdVvCbnkrc5cYqN4l01elqr3MSsj6BJ+JqIqViFrYYkD34THKO8c+wZGb8IN+NJAVre9YOvt5+Cvbbd5ik0UQ+YQNM=",
                ReadCapability = NabVelocity.Svc.ReadCapability.KeyOnly,
                SerialNumber = "208093707",
                SoftwareVersion = "1.0",
                SoftwareVersionDate = new DateTime(2014, 12, 4),
            };

            string applicationProfileId = "";

            try
            {
                // an application identified is returned and passed in on every transaction
                applicationProfileId = svcClient.SaveApplicationData(sessionToken, applicationData);
            }
            catch (FaultException<NabVelocity.Svc.CWSValidationResultFault> ex)
            {
                foreach (var validationError in ex.Detail.Errors)
                {
                    Console.WriteLine(string.Format("Validatior error: {0} - {1}",
                        validationError.RuleLocationKey, validationError.RuleMessage));
                }
            }

            #endregion

            #region GetApplicationData

            ApplicationData appData = svcClient.GetApplicationData(sessionToken, applicationProfileId);

            #endregion

            #region GetServiceInformation

            ServiceInformation serviceInfo = svcClient.GetServiceInformation(sessionToken);
            string serviceId = "";

            bool imageTransactions = true;
            if (serviceInfo.ElectronicCheckingServices.Count() > 0)
            {
                if (serviceInfo.ElectronicCheckingServices.First().ServiceName.Contains("GETI"))
                    imageTransactions = true;

                serviceId = serviceInfo.ElectronicCheckingServices.First().ServiceId;
            }

            bool serviceIsHostCapture = false;
            bool serviceIsTermCapture = false;

            if (serviceInfo.BankcardServices.Count() > 0)
            {
                BankcardService service = serviceInfo.BankcardServices.First();
                //// the serviceId represents the payment processor (global, firstdata, chase, etc.)
                serviceId = service.ServiceId;
                // if Capture is supported, the service is host capture
                serviceIsHostCapture = service.Operations.Capture;
                // if CaptureAll is supprted, the service is terminal capture
                serviceIsTermCapture = service.Operations.CaptureAll;
            }
            #endregion

            #region SaveMerchantProfiles

            string merchantProfileId = "Joe's Online Crab Shack";
            var merchantProfile = new MerchantProfile()
            {
                ProfileId = merchantProfileId,
                ServiceId = serviceId,
                MerchantData = new MerchantProfileMerchantData()
                {
                    MerchantId = "689035621266620",
                    Name = "Joe's Online Crab Shack",
                    Phone = "(555) 555-5555",
                    CustomerServicePhone = "(555) 555-5555",
                    Address = new NabVelocity.Svc.AddressInfo()
                    {
                        Street1 = "100 Rampart Lane",
                        City = "Denver",
                        PostalCode = "80220",
                    },
                    BankcardMerchantData = new BankcardMerchantData()
                    {
                        IndustryType = NabVelocity.Svc.IndustryType.Ecommerce,
                        AgentBank = "000000",
                    },
                },
            };

            try
            {
                // merchant profiles follow the processor, so always use the serviceId (not the workflowId)
                svcClient.SaveMerchantProfiles(sessionToken, serviceId, TenderType.Credit,
                    new MerchantProfile[] { merchantProfile });
            }
            catch (FaultException<NabVelocity.Svc.CWSValidationResultFault> ex)
            {
                foreach (var validationError in ex.Detail.Errors)
                {
                    Console.WriteLine(string.Format("Validatior error: {0} - {1}",
                        validationError.RuleLocationKey, validationError.RuleMessage));
                }
            }

            #endregion

            #region GetMerchantProfiles

            MerchantProfile[] merchantProfiles = svcClient.GetMerchantProfiles(sessionToken, serviceId, TenderType.Credit);

            #endregion

            #endregion

            #region Transacting

            if (serviceInfo.ElectronicCheckingServices.Count() > 0)
            {
                string filePathName = @"..\..\..\tifBackSample.tif";
                byte[] rawImgData = File.ReadAllBytes(filePathName);
                char[] base64CharData = new char[getB64Length(rawImgData.Length)];
                Convert.ToBase64CharArray(rawImgData, 0, rawImgData.Length, base64CharData, 0);
                byte[] backImageData = Encoding.ASCII.GetBytes(base64CharData);

                filePathName = @"..\..\..\tifFrontSample.tif";
                rawImgData = File.ReadAllBytes(filePathName);
                base64CharData = new char[getB64Length(rawImgData.Length)];
                Convert.ToBase64CharArray(rawImgData, 0, rawImgData.Length, base64CharData, 0);
                byte[] frontImageData = Encoding.ASCII.GetBytes(base64CharData);

                var debitRequest = new ElectronicCheckingTransaction
                {
                    CustomerData = new ElectronicCheckingCustomerData()
                    {
                        BillingData = new CustomerInfo()
                        {
                            Address = new NabVelocity.Txn.AddressInfo()
                            {
                                Street1 = "123 N Central",
                                City = "Thornton",
                                PostalCode = "12345",
                                StateProvince = "CO",
                                CountryCode = NabVelocity.Txn.TypeISOCountryCodeA3.USA
                            },
                            Name = new NameInfo
                            {
                                First = "Bob",
                                Last = "Dillan",
                            },
                            Phone = "3213211234"
                        },
                    },
                    TenderData = new ElectronicCheckingTenderData
                    {
                        CheckData = new CheckData
                        {
                            AccountNumber = "12345678",
                            RoutingNumber = "490000018",
                            CheckCountryCode = CheckCountryCode.US,
                            CheckNumber = "1234",
                            OwnerType = OwnerType.Personal,
                            UseType = UseType.Checking,
                            RawMICRLine = "toad"
                        },
                        SocketLocation = new SocketLocation
                        {
                            SocketCity = "Denver",
                            SocketState = "CO"
                        },
                        BackCheckImage = new CheckImage
                        {
                            CompressionType = ImgCompressionType.Group_4,
                            DocType = DocType.Check,
                            FormatType = ImgFormatType.TIFF_6,
                            ImgData = backImageData,
                            ImgSize = backImageData.Length,
                        },
                        FrontCheckImage = new CheckImage
                        {
                            CompressionType = ImgCompressionType.Group_4,
                            DocType = DocType.Check,
                            FormatType = ImgFormatType.TIFF_6,
                            ImgData = frontImageData,
                            ImgSize = frontImageData.Length,
                        }
                    },
                    TransactionData = new ElectronicCheckingTransactionData
                    {
                        Amount = 25.00m,
                        SECCode = SECCode.POP,
                        TransactionType = TransactionType.Debit,
                        CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                        EffectiveDate = DateTime.Now,
                        IsRecurring = false,
                        PayeeId = "Some Data",
                        ServiceType = ServiceType.ACH,
                        TxnCodeType = TxnCodeType.Conversion,
                    }
                };

                try
                {
                    var debitResponse =
                        (ElectronicCheckingTransactionResponse)
                            txnClient.Authorize(sessionToken, debitRequest, applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(Debit) Status: " + debitResponse.Status + "\r\n");
                }
                catch (FaultException<NabVelocity.Txn.CWSValidationResultFault> ex)
                {
                    foreach (var validationError in ex.Detail.Errors)
                    {
                        Console.WriteLine(string.Format("Validatior error: {0} - {1}", validationError.RuleLocationKey,
                            validationError.RuleMessage));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (serviceIsHostCapture)
            {
                #region Host Capture workflow

                try
                {
                    #region Verify

                    var verifyRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "5100000000000016",
                                Expire = "1215",
                            },
                            CardSecurityData = new CardSecurityData()
                            {
                                AVSData = new AVSData()
                                {
                                    Street = "123 Rain Road",
                                    City = "Aurora",
                                    StateProvince = "CO",
                                    PostalCode = "80080",
                                },
                                CVData = "383",
                                CVDataProvided = CVDataProvided.Provided,
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            Amount = 0.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                        CustomerData = new TransactionCustomerData()
                        {
                        }
                    };

                    var verifyResponse = (BankcardTransactionResponse)txnClient.Verify(sessionToken, verifyRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(Verify) Status: " + verifyResponse.Status + "\r\n"
                                    + "CV Result: " + verifyResponse.CVResult + "\r\n"
                                    + "AVS Postal Result: " + verifyResponse.AVSResult.PostalCodeResult + "\r\n");

                    #endregion

                    #region Authorize

                    var authRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "5100000000000016",
                                Expire = "1215",
                            },
                            CardSecurityData = new CardSecurityData()
                            {
                                AVSData = new AVSData()
                                {
                                    Street = "123 Rain Road",
                                    City = "Aurora",
                                    StateProvince = "CO",
                                    PostalCode = "80080",
                                },
                                CVData = "383",
                                CVDataProvided = CVDataProvided.Provided,
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    var authResponse = (BankcardTransactionResponse)txnClient.Authorize(sessionToken, authRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(Authorize) Status: " + authResponse.Status + "\r\n"
                                    + "Amount: " + authResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + authResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + authResponse.TransactionId + "\r\n");

                    #endregion

                    #region Capture

                    var captureDifferenceData = new BankcardCapture()
                    {
                        TransactionId = authResponse.TransactionId,
                        Amount = authResponse.Amount + 1.00M,
                    };

                    var captureResponse = (BankcardCaptureResponse)txnClient.Capture(sessionToken, captureDifferenceData,
                        applicationProfileId, serviceId);

                    Console.WriteLine("(Capture) Status: " + captureResponse.Status + "\r\n"
                                    + "Amount: " + captureResponse.TransactionSummaryData.NetTotals.NetAmount + "\r\n"
                                    + "TransactionId: " + captureResponse.TransactionId + "\r\n");

                    #endregion

                    #region AuthAndCapture

                    var authAndCaptureRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "5100000000000016",
                                Expire = "1215",
                            },
                            CardSecurityData = new CardSecurityData()
                            {
                                AVSData = new AVSData()
                                {
                                    Street = "123 Rain Road",
                                    City = "Aurora",
                                    StateProvince = "CO",
                                    PostalCode = "80080",
                                },
                                CVData = "383",
                                CVDataProvided = CVDataProvided.Provided,
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    var authAndCapResponse = (BankcardTransactionResponse)txnClient.AuthorizeAndCapture(sessionToken, authAndCaptureRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(AuthAndCapture) Status: " + authAndCapResponse.Status + "\r\n"
                                    + "Amount: " + authAndCapResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + authAndCapResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + authAndCapResponse.TransactionId + "\r\n");

                    #endregion

                    #region ReturnById

                    var returnByIdRequest = new BankcardReturn()
                    {
                        TransactionId = authAndCapResponse.TransactionId,
                        TransactionDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
                    };

                    var returnByIdResponse = (BankcardTransactionResponse)txnClient.ReturnById(sessionToken, returnByIdRequest,
                        applicationProfileId, serviceId);

                    Console.WriteLine("(ReturnById) Status: " + returnByIdResponse.Status + "\r\n"
                                    + "Amount: " + returnByIdResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + returnByIdResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + returnByIdResponse.TransactionId + "\r\n");

                    #endregion

                    #region ReturnUnlinked

                    var returnRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "5100000000000016",
                                Expire = "1215",
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    var returnUnlinkedResponse = (BankcardTransactionResponse)txnClient.ReturnUnlinked(sessionToken, returnRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(ReturnUnlinked) Status: " + returnUnlinkedResponse.Status + "\r\n"
                                    + "Amount: " + returnUnlinkedResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + returnUnlinkedResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + returnUnlinkedResponse.TransactionId + "\r\n");

                    #endregion

                    #region Tokenized Transactions

                    // build a transaction
                    var tokenizedRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            // we only need to use a token in the tender data now
                            PaymentAccountDataToken = verifyResponse.PaymentAccountDataToken
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    authResponse = (BankcardTransactionResponse)txnClient.Authorize(sessionToken, tokenizedRequest,
                        applicationProfileId, merchantProfileId, serviceId);
                    authAndCapResponse = (BankcardTransactionResponse)txnClient.AuthorizeAndCapture(sessionToken, tokenizedRequest,
                        applicationProfileId, merchantProfileId, serviceId);
                    returnUnlinkedResponse = (BankcardTransactionResponse)txnClient.ReturnUnlinked(sessionToken, tokenizedRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    #endregion

                    #region Adjust

                    var adjustReq = new Adjust()
                    {
                        Amount = 1.00M,
                        TransactionId = authAndCapResponse.TransactionId,
                    };

                    Response adjustResponse = txnClient.Adjust(sessionToken, adjustReq, applicationProfileId, serviceId);

                    Console.WriteLine("(Adjust) Status: " + adjustResponse.Status + "\r\n"
                                    + "StatusMessage: " + adjustResponse.StatusMessage + "\r\n"
                                    + "TransactionId: " + adjustResponse.TransactionId + "\r\n");

                    #endregion

                    #region Undo

                    var undoRequest = new BankcardUndo()
                    {
                        TransactionId = authResponse.TransactionId,
                    };

                    Response undoResponse = txnClient.Undo(sessionToken, undoRequest, applicationProfileId, serviceId);

                    Console.WriteLine("(Undo) Status: " + undoResponse.Status + "\r\n"
                                    + "StatusMessage: " + undoResponse.StatusMessage + "\r\n"
                                    + "TransactionId: " + undoResponse.TransactionId + "\r\n");

                    #endregion
                }
                catch (FaultException<NabVelocity.Txn.CWSValidationResultFault> ex)
                {
                    foreach (var validationError in ex.Detail.Errors)
                    {
                        Console.WriteLine(string.Format("Validatior error: {0} - {1}",
                            validationError.RuleLocationKey, validationError.RuleMessage));
                    }
                }

                #endregion
            }

            if (serviceIsTermCapture)
            {
                #region Term Capture Workflow

                try
                {
                    #region Verify

                    var verifyRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "4111111111111111",
                                Expire = "1215",
                            },
                            CardSecurityData = new CardSecurityData()
                            {
                                AVSData = new AVSData()
                                {
                                    Street = "123 Rain Road",
                                    City = "Aurora",
                                    StateProvince = "CO",
                                    PostalCode = "80080",
                                },
                                CVData = "123",
                                CVDataProvided = CVDataProvided.Provided,
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            Amount = 0.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                        CustomerData = new TransactionCustomerData()
                        {
                        }
                    };

                    var verifyResponse = (BankcardTransactionResponse)txnClient.Verify(sessionToken, verifyRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(Verify) Status: " + verifyResponse.Status + "\r\n"
                                    + "CV Result: " + verifyResponse.CVResult + "\r\n"
                                    + "AVS Postal Result: " + verifyResponse.AVSResult.PostalCodeResult + "\r\n");

                    #endregion

                    #region Authorize

                    var authRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "4111111111111111",
                                Expire = "1215",
                            },
                            CardSecurityData = new CardSecurityData()
                            {
                                AVSData = new AVSData()
                                {
                                    Street = "123 Rain Road",
                                    City = "Aurora",
                                    StateProvince = "CO",
                                    PostalCode = "80080",
                                },
                                CVData = "123",
                                CVDataProvided = CVDataProvided.Provided,
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    var authResponse = (BankcardTransactionResponse)txnClient.Authorize(sessionToken, authRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(Authorize) Status: " + authResponse.Status + "\r\n"
                                    + "Amount: " + authResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + authResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + authResponse.TransactionId + "\r\n");

                    #endregion

                    #region Capture Selective

                    var captureSelectiveDifferenceData = new BankcardCapture()
                    {
                        TransactionId = authResponse.TransactionId,
                        Amount = authResponse.Amount + 1.00M,
                    };

                    Response[] captureSelectiveResponses = txnClient.CaptureSelective(sessionToken, new[] { authResponse.TransactionId },
                        new[] { captureSelectiveDifferenceData }, applicationProfileId, serviceId);

                    foreach (var response in captureSelectiveResponses)
                    {
                        if (response.Status == Status.Failure)
                        {
                            Console.WriteLine("(Capture Selective) Status: " + response.Status + "\r\n"
                                            + "StatusMessage: " + response.StatusMessage + "\r\n"
                                            + "TransactionId: " + response.TransactionId + "\r\n");
                        }
                        else
                        {
                            var captureResponse = (BankcardCaptureResponse)response;

                            Console.WriteLine("(Capture Selective) Status: " + captureResponse.Status + "\r\n"
                                + "Industry: " + captureResponse.IndustryType + "\r\n"
                                + "Sales Count: " + captureResponse.TransactionSummaryData.SaleTotals.Count + "\r\n"
                                + "Sales Amount: " + captureResponse.TransactionSummaryData.SaleTotals.NetAmount + "\r\n"
                                + "Return Count: " + captureResponse.TransactionSummaryData.ReturnTotals.Count + "\r\n"
                                + "Return Amount: " + captureResponse.TransactionSummaryData.ReturnTotals.NetAmount + "\r\n"
                                + "TransactionId: " + captureResponse.TransactionId + "\r\n");
                        }
                    }

                    #endregion

                    #region ReturnById

                    var returnByIdRequest = new BankcardReturn()
                    {
                        TransactionId = authResponse.TransactionId,
                        TransactionDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
                    };

                    var returnByIdResponse = (BankcardTransactionResponse)txnClient.ReturnById(sessionToken, returnByIdRequest,
                        applicationProfileId, serviceId);

                    Console.WriteLine("(ReturnById) Status: " + returnByIdResponse.Status + "\r\n"
                                    + "Amount: " + returnByIdResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + returnByIdResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + returnByIdResponse.TransactionId + "\r\n");

                    #endregion

                    #region ReturnUnlinked

                    var returnRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            CardData = new CardData()
                            {
                                CardType = TypeCardType.Visa,
                                PAN = "4111111111111111",
                                Expire = "1215",
                            },
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    var returnUnlinkedResponse = (BankcardTransactionResponse)txnClient.ReturnUnlinked(sessionToken, returnRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    Console.WriteLine("(ReturnUnlinked) Status: " + returnUnlinkedResponse.Status + "\r\n"
                                    + "Amount: " + returnUnlinkedResponse.Amount + "\r\n"
                                    + "ApprovalCode: " + returnUnlinkedResponse.ApprovalCode + "\r\n"
                                    + "TransactionId: " + returnUnlinkedResponse.TransactionId + "\r\n");

                    #endregion

                    #region Tokenized Transactions

                    // build a transaction
                    var tokenizedRequest = new BankcardTransaction()
                    {
                        TenderData = new BankcardTenderData()
                        {
                            // we only need to use a token in the tender data now
                            PaymentAccountDataToken = verifyResponse.PaymentAccountDataToken
                        },
                        TransactionData = new BankcardTransactionData()
                        {
                            CurrencyCode = NabVelocity.Txn.TypeISOCurrencyCodeA3.USD,
                            OrderNumber = "123456",
                            Amount = 15.00M,
                            EntryMode = NabVelocity.Txn.EntryMode.Keyed,
                            IndustryType = NabVelocity.Txn.IndustryType.Ecommerce,
                        },
                    };

                    authResponse = (BankcardTransactionResponse)txnClient.Authorize(sessionToken, tokenizedRequest,
                        applicationProfileId, merchantProfileId, serviceId);
                    returnUnlinkedResponse = (BankcardTransactionResponse)txnClient.ReturnUnlinked(sessionToken, tokenizedRequest,
                        applicationProfileId, merchantProfileId, serviceId);

                    #endregion

                    #region Adjust

                    var adjustReq = new Adjust()
                    {
                        Amount = 1.00M,
                        TransactionId = authResponse.TransactionId,
                    };

                    Response adjustResponse = txnClient.Adjust(sessionToken, adjustReq, applicationProfileId, serviceId);

                    Console.WriteLine("(Adjust) Status: " + adjustResponse.Status + "\r\n"
                                    + "StatusMessage: " + adjustResponse.StatusMessage + "\r\n"
                                    + "TransactionId: " + adjustResponse.TransactionId + "\r\n");

                    #endregion

                    #region Undo

                    var undoRequest = new BankcardUndo()
                    {
                        TransactionId = adjustResponse.TransactionId,
                    };

                    Response undoResponse = txnClient.Undo(sessionToken, undoRequest, applicationProfileId, serviceId);

                    Console.WriteLine("(Undo) Status: " + undoResponse.Status + "\r\n"
                                    + "StatusMessage: " + undoResponse.StatusMessage + "\r\n"
                                    + "TransactionId: " + undoResponse.TransactionId + "\r\n");

                    #endregion

                    #region Capture All

                    Response[] captureAllResponses = txnClient.CaptureAll(sessionToken, null,
                        null, applicationProfileId, merchantProfileId, serviceId);

                    foreach (var response in captureAllResponses)
                    {
                        if (response.Status == Status.Failure)
                        {
                            Console.WriteLine("(Capture All) Status: " + response.Status + "\r\n"
                                            + "StatusMessage: " + response.StatusMessage + "\r\n"
                                            + "TransactionId: " + response.TransactionId + "\r\n");
                        }
                        else
                        {
                            var captureResponse = (BankcardCaptureResponse)response;

                            Console.WriteLine("(Capture All) Status: " + captureResponse.Status + "\r\n"
                                + "Industry: " + captureResponse.IndustryType + "\r\n"
                                + "Sales Count: " + captureResponse.TransactionSummaryData.SaleTotals.Count + "\r\n"
                                + "Sales Amount: " + captureResponse.TransactionSummaryData.SaleTotals.NetAmount + "\r\n"
                                + "Return Count: " + captureResponse.TransactionSummaryData.ReturnTotals.Count + "\r\n"
                                + "Return Amount: " + captureResponse.TransactionSummaryData.ReturnTotals.NetAmount + "\r\n"
                                + "TransactionId: " + captureResponse.TransactionId + "\r\n");
                        }
                    }

                    #endregion
                }
                catch (FaultException<NabVelocity.Txn.CWSValidationResultFault> ex)
                {
                    foreach (var validationError in ex.Detail.Errors)
                    {
                        Console.WriteLine(string.Format("Validatior error: {0} - {1}",
                            validationError.RuleLocationKey, validationError.RuleMessage));
                    }
                }

                #endregion
            }

            #endregion

            #region Querying for Transactions

            var tmsClient = new NabVelocity.Tms.TMSOperationsClient();

            var queryParams = new NabVelocity.Tms.QueryTransactionsParameters()
            {
                Amounts = new[] { 16.00M },
                CardTypes = new[] { NabVelocity.Tms.TypeCardType.Visa },
                CaptureStates = new[] { NabVelocity.Tms.CaptureState.Captured },
                MerchantProfileIds = new[] { "My Test App" },
                TransactionDateRange = new NabVelocity.Tms.DateRange() { StartDateTime = DateTime.Now.AddDays(-2), EndDateTime = DateTime.Now },
                TransactionClassTypePairs = new[] { 
                    new NabVelocity.Tms.TransactionClassTypePair() { TransactionClass = "BCP", TransactionType = "AUTH" } 
                },
            };

            var pagingParams = new NabVelocity.Tms.PagingParameters() { Page = 0, PageSize = 10 };

            var txnsSummary = tmsClient.QueryTransactionsSummary(sessionToken, queryParams, false, pagingParams);

            var txnsDetail = tmsClient.QueryTransactionsDetail(sessionToken, queryParams, NabVelocity.Tms.TransactionDetailFormat.CWSTransaction, false, pagingParams);

            var txnsFamilies = tmsClient.QueryTransactionFamilies(sessionToken, queryParams, pagingParams);

            #endregion
        }

        private static int getB64Length(int originalSizeInBytes)
        {
            return 4*(int) Math.Ceiling(originalSizeInBytes/3.0);
        }
    }
}
