using Android.Content;
using Android.Net;
using Android.Telephony;
using AzureOfflineSyncDemo.DI;
using AzureOfflineSyncDemo.Droid.DS;

[assembly: Xamarin.Forms.Dependency(typeof(Connectivity))]
namespace AzureOfflineSyncDemo.Droid.DS
{
    public class Connectivity : IConnectivity
    {

        /**
         * Get the network info
         * @param context
         * @return
         */
        public static NetworkInfo getNetworkInfo(Context context)
        {
            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            return cm.ActiveNetworkInfo;
        }

        /**
         * Check if there is any connectivity
         * @param context
         * @return
         */
        public static bool isConnected(Context context)
        {
            NetworkInfo info = Connectivity.getNetworkInfo(context);
            return (info != null && info.IsConnected);
        }

        /**
         * Check if there is any connectivity to a Wifi network
         * @param context
         * @param type
         * @return
         */
        public static bool IsConnectedWifi(Context context)
        {
            NetworkInfo info = Connectivity.getNetworkInfo(context);
            return (info != null && info.IsConnected && info.Type == ConnectivityType.Wifi);
        }

        /**
         * Check if there is any connectivity to a mobile network
         * @param context
         * @param type
         * @return
         */
        public static bool IsConnectedMobile(Context context)
        {
            NetworkInfo info = Connectivity.getNetworkInfo(context);
            return (info != null && info.IsConnected && info.Type == ConnectivityType.Mobile);
        }

        /**
         * Check if there is fast connectivity
         * @param context
         * @return
         */
        public static bool IsConnectedFast(Context context)
        {
            NetworkInfo info = Connectivity.getNetworkInfo(context);
            return (info != null && info.IsConnected && Connectivity.IsConnectionFast(info.Type, info.Subtype));
        }

        /**
         * Check if the connection is fast
         * @param type
         * @param subType
         * @return
         */
        public static bool IsConnectionFast(ConnectivityType type, ConnectivityType subType)
        {
            if (type == ConnectivityType.Wifi)
            {
                return true;
            }
            else if (type == ConnectivityType.Mobile)
            {
                switch ((int)subType)
                {
                    case (int)NetworkType.OneXrtt:
                        return false; // ~ 50-100 kbps
                    case (int)NetworkType.Cdma:
                        return false; // ~ 14-64 kbps
                    case (int)NetworkType.Edge:
                        return false; // ~ 50-100 kbps
                    case (int)NetworkType.Evdo0:
                        return true; // ~ 400-1000 kbps
                    case (int)NetworkType.EvdoA:
                        return true; // ~ 600-1400 kbps
                    case (int)NetworkType.Gprs:
                        return false; // ~ 100 kbps
                    case (int)NetworkType.Hsdpa:
                        return true; // ~ 2-14 Mbps
                    case (int)NetworkType.Hspa:
                        return true; // ~ 700-1700 kbps
                    case (int)NetworkType.Hsupa:
                        return true; // ~ 1-23 Mbps
                    case (int)NetworkType.Umts:
                        return true; // ~ 400-7000 kbps
                                     /*
                                      * Above API level 7, make sure to set android:targetSdkVersion 
                                      * to appropriate level to use these
                                      */
                    case (int)NetworkType.Ehrpd: // API level 11 
                        return true; // ~ 1-2 Mbps
                    case (int)NetworkType.EvdoB: // API level 9
                        return true; // ~ 5 Mbps
                    case (int)NetworkType.Hspap: // API level 13
                        return true; // ~ 10-20 Mbps
                    case (int)NetworkType.Iden: // API level 8
                        return false; // ~25 kbps 
                    case (int)NetworkType.Lte: // API level 11
                        return true; // ~ 10+ Mbps
                                     // Unknown
                    case (int)NetworkType.Unknown:
                        return false;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsFastInternet()
        {
            return IsConnectedFast(Xamarin.Forms.Forms.Context);
        }
    }
}