using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class adcData
    {
        private static readonly int filterLength = 30;

        public static int currentTime_mc;
        public static int[] currentAdc = new int[16];
        public static int[] olderAdc = new int[16];
        public static int[] zeroAdc = new int[16];
        public static int[] zeroedAdc = new int[16];
        public static int[,] filterBuff = new int[16, filterLength];
        public static int[] filteredAdc = new int[16];
        public static int[] middledAdc = new int[16];
        public static int[] absAdc = new int[16];


        public static int[] weights = new int[4];
        public static int[] diffX = new int[4];
        public static int[] diffY = new int[4];

        //public static float[] weights = new int[4];
        //public static float[] diffX = new int[4];
        //public static float[] diffY = new int[4];



        public static void filterData()
        {
            long[] sums = new long[16];
            int tmp;
            for (int j = 0; j < 16; j++)
            {
                for (int i = 1; i < filterLength; i++)
                {
                    filterBuff[j, i - 1] = filterBuff[j, i];
                }
                filterBuff[j, filterLength-1] = zeroedAdc[j];
            }

            for (int j = 0; j < 16; j++)
            {
                sums[j] = 0; tmp = 0;
                for (int i = 0; i < filterLength; i++)
                {
                    tmp++;
                    sums[j] += filterBuff[j, i] * tmp;
                }
                filteredAdc[j] = (int)(sums[j] / tmp);
                absAdc[j] = Math.Abs(filteredAdc[j]);
            }

            for (int j = 0; j < 4; j++) { weights[j] = 0; for (int i = 0; i < 4; i++) weights[j] += absAdc[i+j*4]; };
            
            for (int j = 0; j < 4; j++) 
            {
                diffX[j] = (absAdc[j * 4 + 0] + absAdc[j * 4 + 3]) - (absAdc[j * 4 + 1] + absAdc[j * 4 + 2]);
                diffY[j] = (absAdc[j * 4 + 0] + absAdc[j * 4 + 1]) - (absAdc[j * 4 + 2] + absAdc[j * 4 + 3]);
            };


        }
        public static void freshData()
        { 
            for (int i = 0; i < currentAdc.Length; i++) { zeroedAdc[i] =  currentAdc[i] - zeroAdc[i]; middledAdc[i] = (int)((filterLength * middledAdc[i] + currentAdc[i]) / (filterLength+1)); }
            
            filterData();

            // write to storage here
        }


        public static void Zero()
        {
            for (int i = 0; i < currentAdc.Length; i++) { zeroAdc[i] = middledAdc[i]; }





        }
    }
}



public static class DataStorage
{
    


}

