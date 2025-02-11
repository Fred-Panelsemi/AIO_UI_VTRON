using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIO
{
    class mformula
    {
        static float A;
        static float B;
        static float C;
        static float delta;
        static float x1, x2, x3;
        static float x2_real, x3_real, x2_virtual, x3_virtual;
        static float deltaQuadratic;

        /// <summary>
        /// 盛金公式求解一元三次方程
        /// </summary>
        /// <param name="a">三次系数</param>
        /// <param name="b">二次系数</param>
        /// <param name="c">一次系数</param>
        /// <param name="d">常系数</param>
        /// <param name="r1">结果1 为null则没有实根</param>
        /// <param name="r2">结果2</param>
        /// <param name="r3">结果3</param>
        public static void solveEquations(float a, float b, float c, float d, out float r1, out float r2, out float r3)
        {
            r1 = 0;
            r2 = 0;
            r3 = 0;

            A = b * b - 3 * a * c;
            B = b * c - 9 * a * d;
            C = c * c - 3 * b * d;
            delta = B * B - 4 * A * C;

            if (d == 0)
            {
                x3 = 0;
                solveQuadraticEquation(a, b, c, d);
            }
            else if (A == 0 && B == 0)
            {
                x1 = -b / (3 * a);
                x2 = -c / b;
                x3 = -3 * d / c;

            }
            else if (delta > 0)
            {
                float Y1 = float.Parse((A * b + 3 * a * (-B + Math.Sqrt(delta)) / 2).ToString());
                float Y2 = float.Parse((A * b + 3 * a * (-B - Math.Sqrt(delta)) / 2).ToString());

                x1 = (-b - (getCubeRoot(Y1) + getCubeRoot(Y2))) / (3 * a);
                x3_real = x2_real = (-b + getCubeRoot(Y1)) / (3 * a);
                x2_virtual = float.Parse((((Math.Sqrt(3) / 2) * (getCubeRoot(Y1) - getCubeRoot(Y2))) / (3 * a)).ToString());
                x3_virtual = -x2_virtual;
            }
            else if (delta == 0)
            {
                var K = B / A; //A != 0
                x1 = -b / a + K;
                x2 = x3 = -K / 2;
            }
            else
            {
                //delta < 0
                var T = (2 * A * b - 3 * a * B) / (2 * Math.Sqrt(A * A * A)); // A > 0, -1 < T < 1
                var angle = Math.Acos(T) / 3;
                x1 = float.Parse(((-b - 2 * Math.Sqrt(A) * Math.Cos(angle)) / (3 * a)).ToString());
                x2 = float.Parse(((-b + Math.Sqrt(A) * (Math.Cos(angle) + Math.Sqrt(3) * Math.Sin(angle))) / (3 * a)).ToString());
                x3 = float.Parse(((-b + Math.Sqrt(A) * (Math.Cos(angle) - Math.Sqrt(3) * Math.Sin(angle))) / (3 * a)).ToString());
            }

            if (A == 0 && B == 0)
            {
                //方程有一个三重实根
                r1 = x1;
                r2 = x1;
                r3 = x1;
                return;
            }
            else if (delta > 0)
            {
                //方程有一个实根和一对共轭虚根
                r1 = x1;
                r2 = x2_real + x2_virtual;
                r3 = x2_real + x2_virtual;
            }
            else if (delta == 0)
            {
                //方程有一个实根，其中有一个两重根
                r1 = x1;
                r2 = x2;
                r3 = x2;
            }
            else
            {
                //方程有三个不相等的实根
                r1 = x1;
                r2 = x2;
                r3 = x3;
            }
        }

        private static void solveQuadraticEquation(float a, float b, float c, float d)
        {
            deltaQuadratic = b * b - 4 * a * c;
            if (deltaQuadratic == 0)
            {
                x1 = x2 = -b / (2 * a);
            }
            else if (deltaQuadratic > 0)
            {
                x1 = -b + float.Parse((Math.Sqrt(deltaQuadratic) / (2 * a)).ToString());
                x2 = -b - float.Parse((Math.Sqrt(deltaQuadratic) / (2 * a)).ToString());
            }
            else
            {
                x2_real = -b / (2 * a);
                x2_virtual = float.Parse((Math.Sqrt(-deltaQuadratic) / (2 * a)).ToString());
            }
        }

        private static float getCubeRoot(float value)
        {
            if (value < 0)
            {
                return float.Parse((-Math.Pow(-value, 1 / 3)).ToString());
            }
            else if (value == 0)
            {
                return 0;
            }
            else
            {
                return float.Parse(Math.Pow(value, 1 / 3).ToString());
            }
        }







    }

    class invma
    {
        public static double[] act =
        {
            2.39874876653E-01,
            2.58472384421E-04,
            1.34344078332E-06,
            4.02389658381E-09,
            -3.15432392250E-11,
            6.94676766012E-14,
            -7.06834482895E-17,
            2.86382595062E-20
        };

        public static double[] bct =
        {
            -4.83978987580E-01,
            4.92698756749E+00,
            -9.03025039733E+00,
            -2.28049373410E+00,
            4.03935141040E+01,
            -6.68016769618E+01,
            3.46446863883E+01,
        };

        //public static double[] bct =
        //{
        //    -4.83978987580E-01,
        //    4.92698756749E+00,
        //    -9.03025039733E+00,
        //    -2.28049373410E+00,
        //    4.03935141040E+01,
        //    -6.68016769618E+01,
        //    3.46446863883E+01,
        //};

        public static bool MCinv(int n,ref double[,] mtxAR,ref double[,] mtxAI)
        {
            int[] nIs = new int[n];
            int[] nJs = new int[n];
            int i; int j; int k;
            double d; double p; double s; double t; double q; double b;

            #region from matrix 1 disabled backup
            /*
            for (k = 1; k <= n; k++)
            {
                d = 0;
                for (i = k; i <= n; i++)
                {
                    for (j = k; j <= n; j++)
                    {
                        p = mtxAR[i, j] * mtxAR[i, j] + mtxAI[i, j] * mtxAI[i, j];
                        if (p > d)
                        {
                            d = p;
                            nIs[k] = i;
                            nJs[k] = j;
                        }
                    }
                }
                if (d + 1 == 1) { return false; }

                if (nIs[k] != k)
                {
                    for (j = 1; j <= n; j++)
                    {
                        t = mtxAR[k, j];
                        mtxAR[k, j] = mtxAR[nIs[k], j];
                        mtxAR[nIs[k], j] = t;
                        t = mtxAI[k, j];
                        mtxAI[k, j] = mtxAI[nIs[k], j];
                        mtxAI[nIs[k], j] = t;
                    }
                }

                if (nJs[k] != k)
                {
                    for (i = 1; i <= n; i++)
                    {
                        t = mtxAR[i, k];
                        mtxAR[i, k] = mtxAR[i, nJs[k]];
                        mtxAR[i, nJs[k]] = t;
                        t = mtxAI[i, k];
                        mtxAI[i, k] = mtxAI[i, nJs[k]];
                        mtxAI[i, nJs[k]] = t;
                    }
                }

                mtxAR[k, k] = mtxAR[k, k] / d;
                mtxAI[k, k] = -mtxAI[k, k] / d;
                for (j = 1; j <= n; j++)
                {
                    if (j != k)
                    {
                        p = mtxAR[k, j] * mtxAR[k, k];
                        q = mtxAI[k, j] * mtxAI[k, k];
                        s = (mtxAR[k, j] + mtxAI[k, j]) * (mtxAR[k, k] + mtxAI[k, k]);
                        mtxAR[k, j] = p - q;
                        mtxAI[k, j] = s - p - q;
                    }
                }

                for (i = 1; i <= n; i++)
                {
                    if (i != k)
                    {
                        for (j = 1; j <= n; j++)
                        {
                            if (j != k)
                            {
                                p = mtxAR[k, j] * mtxAR[i, k];
                                q = mtxAI[k, j] * mtxAI[i, k];
                                s = (mtxAR[k, j] + mtxAI[k, j]) * (mtxAR[i, k] + mtxAI[i, k]);
                                t = p - q;
                                b = s - p - q;
                                mtxAR[i, j] = mtxAR[i, j] - t;
                                mtxAI[i, j] = mtxAI[i, j] - b;
                            }
                        }
                    }
                }

                for (i = 1; i <= n; i++)
                {
                    if (i != k)
                    {
                        p = mtxAR[i, k] * mtxAR[k, k];
                        q = mtxAI[i, k] * mtxAI[k, k];
                        s = (mtxAR[i, k] + mtxAI[i, k]) * (mtxAR[k, k] + mtxAI[k, k]);
                        mtxAR[i, k] = q - p;
                        mtxAI[i, k] = p + q - s;
                    }
                }
            }

            for (k = n; k >= 1; k--)
            {
                if (nJs[k] != k)
                {
                    for (j = 1; j <= n; j++)
                    {
                        t = mtxAR[k, j];
                        mtxAR[k, j] = mtxAR[nJs[k], j];
                        mtxAR[nJs[k], j] = t;
                        t = mtxAI[k, j];
                        mtxAI[k, j] = mtxAI[nJs[k], j];
                        mtxAI[nJs[k], j] = t;
                    }
                }
                if (nIs[k] != k)
                {
                    for (i = 1; i <= n; i++)
                    {
                        t = mtxAR[i, k];
                        mtxAR[i, k] = mtxAR[i, nIs[k]];
                        mtxAR[i, nIs[k]] = t;
                        t = mtxAI[i, k];
                        mtxAI[i, k] = mtxAI[i, nIs[k]];
                        mtxAI[i, nIs[k]] = t;
                    }
                }
            }
            */
            #endregion from matrix 1

            for (k = 0; k < n; k++)
            {
                d = 0;
                for (i = k; i < n; i++)
                {
                    for (j = k; j < n; j++)
                    {
                        p = mtxAR[i, j] * mtxAR[i, j] + mtxAI[i, j] * mtxAI[i, j];
                        if (p > d)
                        {
                            d = p;
                            nIs[k] = i;
                            nJs[k] = j;
                        }
                    }
                }
                if (d + 1 == 1) { return false; }

                if (nIs[k] != k)
                {
                    for (j = 0; j < n; j++)
                    {
                        t = mtxAR[k, j];
                        mtxAR[k, j] = mtxAR[nIs[k], j];
                        mtxAR[nIs[k], j] = t;
                        t = mtxAI[k, j];
                        mtxAI[k, j] = mtxAI[nIs[k], j];
                        mtxAI[nIs[k], j] = t;
                    }
                }

                if (nJs[k] != k)
                {
                    for (i = 0; i < n; i++)
                    {
                        t = mtxAR[i, k];
                        mtxAR[i, k] = mtxAR[i, nJs[k]];
                        mtxAR[i, nJs[k]] = t;
                        t = mtxAI[i, k];
                        mtxAI[i, k] = mtxAI[i, nJs[k]];
                        mtxAI[i, nJs[k]] = t;
                    }
                }

                mtxAR[k, k] = mtxAR[k, k] / d;
                mtxAI[k, k] = -mtxAI[k, k] / d;
                for (j = 0; j < n; j++)
                {
                    if (j != k)
                    {
                        p = mtxAR[k, j] * mtxAR[k, k];
                        q = mtxAI[k, j] * mtxAI[k, k];
                        s = (mtxAR[k, j] + mtxAI[k, j]) * (mtxAR[k, k] + mtxAI[k, k]);
                        mtxAR[k, j] = p - q;
                        mtxAI[k, j] = s - p - q;
                    }
                }

                for (i = 0; i < n; i++)
                {
                    if (i != k)
                    {
                        for (j = 0; j < n; j++)
                        {
                            if (j != k)
                            {
                                p = mtxAR[k, j] * mtxAR[i, k];
                                q = mtxAI[k, j] * mtxAI[i, k];
                                s = (mtxAR[k, j] + mtxAI[k, j]) * (mtxAR[i, k] + mtxAI[i, k]);
                                t = p - q;
                                b = s - p - q;
                                mtxAR[i, j] = mtxAR[i, j] - t;
                                mtxAI[i, j] = mtxAI[i, j] - b;
                            }
                        }
                    }
                }

                for (i = 0; i < n; i++)
                {
                    if (i != k)
                    {
                        p = mtxAR[i, k] * mtxAR[k, k];
                        q = mtxAI[i, k] * mtxAI[k, k];
                        s = (mtxAR[i, k] + mtxAI[i, k]) * (mtxAR[k, k] + mtxAI[k, k]);
                        mtxAR[i, k] = q - p;
                        mtxAI[i, k] = p + q - s;
                    }
                }
            }

            for (k = n - 1; k >= 0; k--)
            {
                if (nJs[k] != k)
                {
                    for (j = 0; j < n; j++)
                    {
                        t = mtxAR[k, j];
                        mtxAR[k, j] = mtxAR[nJs[k], j];
                        mtxAR[nJs[k], j] = t;
                        t = mtxAI[k, j];
                        mtxAI[k, j] = mtxAI[nJs[k], j];
                        mtxAI[nJs[k], j] = t;
                    }
                }
                if (nIs[k] != k)
                {
                    for (i = 0; i < n; i++)
                    {
                        t = mtxAR[i, k];
                        mtxAR[i, k] = mtxAR[i, nIs[k]];
                        mtxAR[i, nIs[k]] = t;
                        t = mtxAI[i, k];
                        mtxAI[i, k] = mtxAI[i, nIs[k]];
                        mtxAI[i, nIs[k]] = t;
                    }
                }
            }


            return true;
        }

        public static bool CWinv(double[,] m, ref double[,] c)
        {
            /*
            b1=m[0,0]
            b2=m[1,0]
            b3=m[2,0]
            b4=m[0,1]
            b5=m[1,1]
            b6=m[2,1]
            b7=m[0,2]
            b8=m[1,2]
            b9=m[2,2]
                   det = b1     * b5      * b9      + b2      * b6      * b7      + b3      * b4      * b8      - b1      * b6      * b8      - b2      * b4      * b9      - b3      * b5      * b7;
            */
            double det = m[0,0] * m[1, 1] * m[2, 2] + m[1, 0] * m[2, 1] * m[0, 2] + m[2, 0] * m[0, 1] * m[1, 2] - m[0, 0] * m[2, 1] * m[1, 2] - m[1, 0] * m[0, 1] * m[2, 2] - m[2, 0] * m[1, 1] * m[0, 2];

            c[0, 0] = (m[1, 1] * m[2, 2] - m[2, 1] * m[1, 2]) / det;
            c[1, 0] = (m[2, 0] * m[1, 2] - m[1, 0] * m[2, 2]) / det;
            c[2, 0] = (m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1]) / det;
            c[0, 1] = (m[2, 1] * m[0, 2] - m[0, 1] * m[2, 2]) / det;
            c[1, 1] = (m[0, 0] * m[2, 2] - m[2, 0] * m[0, 2]) / det;
            c[2, 1] = (m[2, 0] * m[0, 1] - m[0, 0] * m[2, 1]) / det;
            c[0, 2] = (m[0, 1] * m[1, 2] - m[1, 1] * m[0, 2]) / det;
            c[1, 2] = (m[1, 0] * m[0, 2] - m[0, 0] * m[1, 2]) / det;
            c[2, 2] = (m[0, 0] * m[1, 1] - m[1, 0] * m[0, 1]) / det;

            return true;
        }

        public static bool CWCT2xy(ref float svxct, ref float svyct, int CT)
        {
            double sv6 = Math.Pow(10, 6);
            svxct = (float)Math.Round(
                (act[7] * Math.Pow(sv6 / CT, 7)) +
                (act[6] * Math.Pow(sv6 / CT, 6)) +
                (act[5] * Math.Pow(sv6 / CT, 5)) +
                (act[4] * Math.Pow(sv6 / CT, 4)) +
                (act[3] * Math.Pow(sv6 / CT, 3)) +
                (act[2] * Math.Pow(sv6 / CT, 2)) +
                (act[1] * (sv6 / CT)) +
                 act[0], 4);
            svyct = (float)Math.Round(
                (bct[6] * Math.Pow(svxct, 6)) +
                (bct[5] * Math.Pow(svxct, 5)) +
                (bct[4] * Math.Pow(svxct, 4)) +
                (bct[3] * Math.Pow(svxct, 3)) +
                (bct[2] * Math.Pow(svxct, 2)) +
                (bct[1] * svxct) +
                 bct[0], 4);

            return true;
        }

        public static bool CWCT2gRGBratio(float svperc, ref float svxct, ref float svyct, ref int gRratio, ref int gGratio, ref int gBratio)
        {
            float zr = 1 - Convert.ToSingle(uc_PictureAdjust.pidxy[0]) - Convert.ToSingle(uc_PictureAdjust.pidxy[1]);
            float zg = 1 - Convert.ToSingle(uc_PictureAdjust.pidxy[2]) - Convert.ToSingle(uc_PictureAdjust.pidxy[3]);
            float zb = 1 - Convert.ToSingle(uc_PictureAdjust.pidxy[4]) - Convert.ToSingle(uc_PictureAdjust.pidxy[5]);
            float xw = Convert.ToSingle(uc_PictureAdjust.pidxy[6]);
            float yw = Convert.ToSingle(uc_PictureAdjust.pidxy[7]);
            float zw = 1 - xw - yw;
            float svzct = 1 - svxct - svyct;

            double[,] SvAR = new double[3, 3];
            double[,] SvAI = new double[3, 3];


            SvAR[0, 0] = Convert.ToSingle(uc_PictureAdjust.pidxy[0]) / Convert.ToSingle(uc_PictureAdjust.pidxy[1]);
            SvAR[1, 0] = Convert.ToSingle(uc_PictureAdjust.pidxy[2]) / Convert.ToSingle(uc_PictureAdjust.pidxy[3]);
            SvAR[2, 0] = Convert.ToSingle(uc_PictureAdjust.pidxy[4]) / Convert.ToSingle(uc_PictureAdjust.pidxy[5]);

            SvAR[0, 1] = 1;
            SvAR[1, 1] = 1;
            SvAR[2, 1] = 1;

            SvAR[0, 2] = zr / Convert.ToSingle(uc_PictureAdjust.pidxy[1]);
            SvAR[1, 2] = zg / Convert.ToSingle(uc_PictureAdjust.pidxy[3]);
            SvAR[2, 2] = zb / Convert.ToSingle(uc_PictureAdjust.pidxy[5]);


            bool SvB = invma.CWinv(SvAR, ref SvAI);

            float[] c = 
            {
                (float)SvAI[0,0], (float)SvAI[1, 0], (float)SvAI[2, 0],
                (float)SvAI[0,1], (float)SvAI[1, 1], (float)SvAI[2, 1],
                (float)SvAI[0,2], (float)SvAI[1, 2], (float)SvAI[2, 2]
            };

            float yr_pid = (c[0] * xw + c[1] * yw + c[2] * zw) / yw;
            float yg_pid = (c[3] * xw + c[4] * yw + c[5] * zw) / yw;
            float yb_pid = (c[6] * xw + c[7] * yw + c[8] * zw) / yw;

            float yr_ct = (c[0] * svxct + c[1] * svyct + c[2] * svzct) / svyct;
            float yg_ct = (c[3] * svxct + c[4] * svyct + c[5] * svzct) / svyct;
            float yb_ct = (c[6] * svxct + c[7] * svyct + c[8] * svzct) / svyct;

            uc_PictureAdjust.rratio = yr_ct / yr_pid;
            uc_PictureAdjust.gratio = yg_ct / yg_pid;
            uc_PictureAdjust.bratio = yb_ct / yb_pid;

            float dmin = uc_PictureAdjust.rratio;
            if (uc_PictureAdjust.rratio > uc_PictureAdjust.gratio) { if (uc_PictureAdjust.gratio > uc_PictureAdjust.bratio) dmin = uc_PictureAdjust.bratio; else dmin = uc_PictureAdjust.gratio; }
            else { if (uc_PictureAdjust.rratio > uc_PictureAdjust.bratio) dmin = uc_PictureAdjust.bratio; else dmin = uc_PictureAdjust.rratio; }
            if (dmin < 0) { uc_PictureAdjust.rratio -= dmin; uc_PictureAdjust.gratio -= dmin; uc_PictureAdjust.bratio -= dmin; }

            float Amax = uc_PictureAdjust.rratio;
            if (uc_PictureAdjust.rratio > uc_PictureAdjust.gratio) { if (uc_PictureAdjust.rratio > uc_PictureAdjust.bratio) Amax = uc_PictureAdjust.rratio; else Amax = uc_PictureAdjust.bratio; }
            else { if (uc_PictureAdjust.gratio > uc_PictureAdjust.bratio) Amax = uc_PictureAdjust.gratio; else Amax = uc_PictureAdjust.bratio; }
            uc_PictureAdjust.rratio /= Amax; uc_PictureAdjust.gratio /= Amax; uc_PictureAdjust.bratio /= Amax;

            svperc = svperc / 100;

            gRratio = Convert.ToUInt16(string.Format("{0:#00}", (Math.Pow(uc_PictureAdjust.rratio * svperc, (1 / uc_PictureAdjust.pidgv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax))));
            gGratio = Convert.ToUInt16(string.Format("{0:#00}", (Math.Pow(uc_PictureAdjust.gratio * svperc, (1 / uc_PictureAdjust.pidgv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax))));
            gBratio = Convert.ToUInt16(string.Format("{0:#00}", (Math.Pow(uc_PictureAdjust.bratio * svperc, (1 / uc_PictureAdjust.pidgv)) * Convert.ToSingle(uc_PictureAdjust.lvgraymax))));

            return true;
        }

        public static bool CWWPxy(ref float svxct, ref float svyct)
        {
            float xw = uc_PictureAdjust.pidxy[6];
            float yw = uc_PictureAdjust.pidxy[7];

            float zr = 1 - uc_PictureAdjust.xr - uc_PictureAdjust.yr;
            float zg = 1 - uc_PictureAdjust.xg - uc_PictureAdjust.yg;
            float zb = 1 - uc_PictureAdjust.xb - uc_PictureAdjust.yb;
            float zw = 1 - xw - yw;

            double[,] SvAR = new double[3, 3];
            double[,] SvAI = new double[3, 3];

            SvAR[0, 0] = uc_PictureAdjust.xr;
            SvAR[1, 0] = uc_PictureAdjust.xg;
            SvAR[2, 0] = uc_PictureAdjust.xb;

            SvAR[0, 1] = uc_PictureAdjust.yr;
            SvAR[1, 1] = uc_PictureAdjust.yg;
            SvAR[2, 1] = uc_PictureAdjust.yb;

            SvAR[0, 2] = zr;
            SvAR[1, 2] = zg;
            SvAR[2, 2] = zb;
            bool SvB = invma.CWinv(SvAR, ref SvAI);

            float ixr = (float)SvAI[0, 0];
            float ixg = (float)SvAI[1, 0];
            float ixb = (float)SvAI[2, 0];
            float iyr = (float)SvAI[0, 1];
            float iyg = (float)SvAI[1, 1];
            float iyb = (float)SvAI[2, 1];
            float izr = (float)SvAI[0, 2];
            float izg = (float)SvAI[1, 2];
            float izb = (float)SvAI[2, 2];

            float kr = (ixr * xw + ixg * yw + ixb * zw) / yw;
            float kg = (iyr * xw + iyg * yw + iyb * zw) / yw;
            float kb = (izr * xw + izg * yw + izb * zw) / yw;

            float X = kr * uc_PictureAdjust.xr * uc_PictureAdjust.rratio + kg * uc_PictureAdjust.xg * uc_PictureAdjust.gratio + kb * uc_PictureAdjust.xb * uc_PictureAdjust.bratio;
            float Y = kr * uc_PictureAdjust.yr * uc_PictureAdjust.rratio + kg * uc_PictureAdjust.yg * uc_PictureAdjust.gratio + kb * uc_PictureAdjust.yb * uc_PictureAdjust.bratio;
            float Z = kr * zr * uc_PictureAdjust.rratio + kg * zg * uc_PictureAdjust.gratio + kb * zb * uc_PictureAdjust.bratio;

            svxct = Convert.ToSingle(string.Format("{0:###0.0###}", X / (X + Y + Z)));
            svyct = Convert.ToSingle(string.Format("{0:###0.0###}", Y / (X + Y + Z)));


            return true;
        }

        public static bool CWWPxy(ref float svxct, ref float svyct, float svrratio, float svgratio, float svbratio)
        {
            float xw = uc_PictureAdjust.pidxy[6];
            float yw = uc_PictureAdjust.pidxy[7];

            float zr = 1 - uc_PictureAdjust.xr - uc_PictureAdjust.yr;
            float zg = 1 - uc_PictureAdjust.xg - uc_PictureAdjust.yg;
            float zb = 1 - uc_PictureAdjust.xb - uc_PictureAdjust.yb;
            float zw = 1 - xw - yw;

            double[,] SvAR = new double[3, 3];
            double[,] SvAI = new double[3, 3];

            SvAR[0, 0] = uc_PictureAdjust.xr;
            SvAR[1, 0] = uc_PictureAdjust.xg;
            SvAR[2, 0] = uc_PictureAdjust.xb;

            SvAR[0, 1] = uc_PictureAdjust.yr;
            SvAR[1, 1] = uc_PictureAdjust.yg;
            SvAR[2, 1] = uc_PictureAdjust.yb;

            SvAR[0, 2] = zr;
            SvAR[1, 2] = zg;
            SvAR[2, 2] = zb;
            bool SvB = invma.CWinv(SvAR, ref SvAI);

            float ixr = (float)SvAI[0, 0];
            float ixg = (float)SvAI[1, 0];
            float ixb = (float)SvAI[2, 0];
            float iyr = (float)SvAI[0, 1];
            float iyg = (float)SvAI[1, 1];
            float iyb = (float)SvAI[2, 1];
            float izr = (float)SvAI[0, 2];
            float izg = (float)SvAI[1, 2];
            float izb = (float)SvAI[2, 2];

            float kr = (ixr * xw + ixg * yw + ixb * zw) / yw;
            float kg = (iyr * xw + iyg * yw + iyb * zw) / yw;
            float kb = (izr * xw + izg * yw + izb * zw) / yw;

            float X = kr * uc_PictureAdjust.xr * svrratio + kg * uc_PictureAdjust.xg * svgratio + kb * uc_PictureAdjust.xb * svbratio;
            float Y = kr * uc_PictureAdjust.yr * svrratio + kg * uc_PictureAdjust.yg * svgratio + kb * uc_PictureAdjust.yb * svbratio;
            float Z = kr * zr * svrratio + kg * zg * svgratio + kb * zb * svbratio;

            svxct = Convert.ToSingle(string.Format("{0:###0.0###}", X / (X + Y + Z)));
            svyct = Convert.ToSingle(string.Format("{0:###0.0###}", Y / (X + Y + Z)));


            return true;
        }

        public static bool CWCGcal(ref string[] svstra)
        {
            float xr = uc_PictureAdjust.xr;
            float yr = uc_PictureAdjust.yr;
            float zr = 1 - xr - yr;
            float xg = uc_PictureAdjust.xg;
            float yg = uc_PictureAdjust.yg;
            float zg = 1 - xg - yg;
            float xb = uc_PictureAdjust.xb;
            float yb = uc_PictureAdjust.yb;
            float zb = 1 - xb - yb;
            float xw = uc_PictureAdjust.xw;
            float yw = uc_PictureAdjust.yw;

            xw = uc_PictureAdjust.xct;
            yw = uc_PictureAdjust.yct;

            float zw = 1 - xw - yw;

            //PID驗證用須關閉
            //xr = 0.6850f;
            //yr = 0.3146f;
            //zr = 1 - (xr + yr);
            //xg = 0.1944f;
            //yg = 0.7481f;
            //zg = 1 - xg - yg;
            //xb = 0.124f;
            //yb = 0.0723f;
            //zb = 1 - xb - yb;
            //xw = 0.2906f;
            //yw = 0.3113f;
            //zw = 1 - xw - yw;

            float xtr = uc_PictureAdjust.pidxy[0];
            float ytr = uc_PictureAdjust.pidxy[1];
            float ztr = 1 - xtr - ytr;
            float xtg = uc_PictureAdjust.pidxy[2];
            float ytg = uc_PictureAdjust.pidxy[3];
            float ztg = 1 - xtg - ytg;
            float xtb = uc_PictureAdjust.pidxy[4];
            float ytb = uc_PictureAdjust.pidxy[5];
            float ztb = 1 - xtb - ytb;
            float xtw = uc_PictureAdjust.xct;
            float ytw = uc_PictureAdjust.yct;
            float ztw = 1 - xtw - ytw;

            if (uc_PictureAdjust.cgkw == "(User)")
            {
                xtr = uc_PictureAdjust.userxy[0];
                ytr = uc_PictureAdjust.userxy[1];
                ztr = 1 - xtr - ytr;
                xtg = uc_PictureAdjust.userxy[2];
                ytg = uc_PictureAdjust.userxy[3];
                ztg = 1 - xtg - ytg;
                xtb = uc_PictureAdjust.userxy[4];
                ytb = uc_PictureAdjust.userxy[5];
                ztb = 1 - xtb - ytb;
                xtw = uc_PictureAdjust.userxy[6];
                ytw = uc_PictureAdjust.userxy[7];
                ztw = 1 - xtw - ytw;
            }
            else if (uc_PictureAdjust.cgkw == "(NTSC)")
            {
                xtr = uc_PictureAdjust.conNTSC[0];
                ytr = uc_PictureAdjust.conNTSC[1];
                ztr = 1 - xtr - ytr;
                xtg = uc_PictureAdjust.conNTSC[2];
                ytg = uc_PictureAdjust.conNTSC[3];
                ztg = 1 - xtg - ytg;
                xtb = uc_PictureAdjust.conNTSC[4];
                ytb = uc_PictureAdjust.conNTSC[5];
                ztb = 1 - xtb - ytb;
                xtw = uc_PictureAdjust.conNTSC[6];
                ytw = uc_PictureAdjust.conNTSC[7];
                ztw = 1 - xtw - ytw;
            }
            else if (uc_PictureAdjust.cgkw == "(PAL)")
            {
                xtr = uc_PictureAdjust.conPAL[0];
                ytr = uc_PictureAdjust.conPAL[1];
                ztr = 1 - xtr - ytr;
                xtg = uc_PictureAdjust.conPAL[2];
                ytg = uc_PictureAdjust.conPAL[3];
                ztg = 1 - xtg - ytg;
                xtb = uc_PictureAdjust.conPAL[4];
                ytb = uc_PictureAdjust.conPAL[5];
                ztb = 1 - xtb - ytb;
                xtw = uc_PictureAdjust.conPAL[6];
                ytw = uc_PictureAdjust.conPAL[7];
                ztw = 1 - xtw - ytw;
            }


            //PAL驗證用須關閉
            //xtr = 0.64f;
            //ytr = 0.33f;
            //ztr = 1 - xtr - ytr;
            //xtg = 0.29f;
            //ytg = 0.6f;
            //ztg = 1 - xtg - ytg;
            //xtb = 0.15f;
            //ytb = 0.06f;
            //ztb = 1 - xtb - ytb;
            //xtw = 0.3127f;
            //ytw = 0.329f;
            //ztw = 1 - xtw - ytw;
            //NTSC驗證用須關閉
            //xtr = 0.67f;
            //ytr = 0.33f;
            //ztr = 1 - xtr - ytr;
            //xtg = 0.21f;
            //ytg = 0.71f;
            //ztg = 1 - xtg - ytg;
            //xtb = 0.14f;
            //ytb = 0.08f;
            //ztb = 1 - xtb - ytb;
            //xtw = 0.3101f;
            //ytw = 0.3162f;
            //ztw = 1 - xtw - ytw;


            double[,] SvAR = new double[3, 3];
            double[,] SvAI = new double[3, 3];

            SvAR[0, 0] = xr;
            SvAR[1, 0] = xg;
            SvAR[2, 0] = xb;
            SvAR[0, 1] = yr;
            SvAR[1, 1] = yg;
            SvAR[2, 1] = yb;
            SvAR[0, 2] = zr;
            SvAR[1, 2] = zg;
            SvAR[2, 2] = zb;
            bool SvB = invma.CWinv(SvAR, ref SvAI);
            float ixr = (float)SvAI[0, 0];
            float ixg = (float)SvAI[1, 0];
            float ixb = (float)SvAI[2, 0];
            float iyr = (float)SvAI[0, 1];
            float iyg = (float)SvAI[1, 1];
            float iyb = (float)SvAI[2, 1];
            float izr = (float)SvAI[0, 2];
            float izg = (float)SvAI[1, 2];
            float izb = (float)SvAI[2, 2];

            SvAR[0, 0] = xtr;
            SvAR[1, 0] = xtg;
            SvAR[2, 0] = xtb;
            SvAR[0, 1] = ytr;
            SvAR[1, 1] = ytg;
            SvAR[2, 1] = ytb;
            SvAR[0, 2] = ztr;
            SvAR[1, 2] = ztg;
            SvAR[2, 2] = ztb;
            SvB = invma.CWinv(SvAR, ref SvAI);
            float ixtr = (float)SvAI[0, 0];
            float ixtg = (float)SvAI[1, 0];
            float ixtb = (float)SvAI[2, 0];
            float iytr = (float)SvAI[0, 1];
            float iytg = (float)SvAI[1, 1];
            float iytb = (float)SvAI[2, 1];
            float iztr = (float)SvAI[0, 2];
            float iztg = (float)SvAI[1, 2];
            float iztb = (float)SvAI[2, 2];

            float kr = (ixr * xw + ixg * yw + ixb * zw) / yw;
            float kg = (iyr * xw + iyg * yw + iyb * zw) / yw;
            float kb = (izr * xw + izg * yw + izb * zw) / yw;

            float ktr = (ixtr * xtw + ixtg * ytw + ixtb * ztw) / ytw;
            float ktg = (iytr * xtw + iytg * ytw + iytb * ztw) / ytw;
            float ktb = (iztr * xtw + iztg * ytw + iztb * ztw) / ytw;


            SvAR[0, 0] = kr * xr;
            SvAR[1, 0] = kg * xg;
            SvAR[2, 0] = kb * xb;
            SvAR[0, 1] = kr * yr;
            SvAR[1, 1] = kg * yg; 
            SvAR[2, 1] = kb * yb;
            SvAR[0, 2] = kr * zr;
            SvAR[1, 2] = kg * zg;
            SvAR[2, 2] = kb * zb;
            SvB = invma.CWinv(SvAR, ref SvAI);
            float ikxr = (float)SvAI[0, 0];
            float ikxg = (float)SvAI[1, 0];
            float ikxb = (float)SvAI[2, 0];
            float ikyr = (float)SvAI[0, 1];
            float ikyg = (float)SvAI[1, 1];
            float ikyb = (float)SvAI[2, 1];
            float ikzr = (float)SvAI[0, 2];
            float ikzg = (float)SvAI[1, 2];
            float ikzb = (float)SvAI[2, 2];

            

            SvAR[0, 0] = Math.Round((ikxr * ktr * xtr + ikxg * ktr * ytr + ikxb * ktr * ztr) * 16384, 0);
            SvAR[1, 0] = Math.Round((ikxr * ktg * xtg + ikxg * ktg * ytg + ikxb * ktg * ztg) * 16384, 0);
            SvAR[2, 0] = Math.Round((ikxr * ktb * xtb + ikxg * ktb * ytb + ikxb * ktb * ztb) * 16384, 0);
            SvAR[0, 1] = Math.Round((ikyr * ktr * xtr + ikyg * ktr * ytr + ikyb * ktr * ztr) * 16384, 0);
            SvAR[1, 1] = Math.Round((ikyr * ktg * xtg + ikyg * ktg * ytg + ikyb * ktg * ztg) * 16384, 0);
            SvAR[2, 1] = Math.Round((ikyr * ktb * xtb + ikyg * ktb * ytb + ikyb * ktb * ztb) * 16384, 0);
            SvAR[0, 2] = Math.Round((ikzr * ktr * xtr + ikzg * ktr * ytr + ikzb * ktr * ztr) * 16384, 0);
            SvAR[1, 2] = Math.Round((ikzr * ktg * xtg + ikzg * ktg * ytg + ikzb * ktg * ztg) * 16384, 0);
            SvAR[2, 2] = Math.Round((ikzr * ktb * xtb + ikzg * ktb * ytb + ikzb * ktb * ztb) * 16384, 0);

            if (SvAR[0, 0] > 32767) { SvAR[0, 0] = 32767; }
            if (SvAR[1, 0] > 32767) { SvAR[1, 0] = 32767; }
            if (SvAR[2, 0] > 32767) { SvAR[2, 0] = 32767; }
            if (SvAR[0, 1] > 32767) { SvAR[0, 1] = 32767; }
            if (SvAR[1, 1] > 32767) { SvAR[1, 1] = 32767; }
            if (SvAR[2, 1] > 32767) { SvAR[2, 1] = 32767; }
            if (SvAR[0, 2] > 32767) { SvAR[0, 2] = 32767; }
            if (SvAR[1, 2] > 32767) { SvAR[1, 2] = 32767; }
            if (SvAR[2, 2] > 32767) { SvAR[2, 2] = 32767; }

            

            if (SvAR[0, 0] < 0) { svstra[0] = (32768 + (int)Math.Abs(SvAR[0, 0])).ToString(); } else { svstra[0] = SvAR[0, 0].ToString(); }
            if (SvAR[1, 0] < 0) { svstra[1] = (32768 + (int)Math.Abs(SvAR[1, 0])).ToString(); } else { svstra[1] = SvAR[1, 0].ToString(); }
            if (SvAR[2, 0] < 0) { svstra[2] = (32768 + (int)Math.Abs(SvAR[2, 0])).ToString(); } else { svstra[2] = SvAR[2, 0].ToString(); }
            if (SvAR[0, 1] < 0) { svstra[3] = (32768 + (int)Math.Abs(SvAR[0, 1])).ToString(); } else { svstra[3] = SvAR[0, 1].ToString(); }
            if (SvAR[1, 1] < 0) { svstra[4] = (32768 + (int)Math.Abs(SvAR[1, 1])).ToString(); } else { svstra[4] = SvAR[1, 1].ToString(); }
            if (SvAR[2, 1] < 0) { svstra[5] = (32768 + (int)Math.Abs(SvAR[2, 1])).ToString(); } else { svstra[5] = SvAR[2, 1].ToString(); }
            if (SvAR[0, 2] < 0) { svstra[6] = (32768 + (int)Math.Abs(SvAR[0, 2])).ToString(); } else { svstra[6] = SvAR[0, 2].ToString(); }
            if (SvAR[1, 2] < 0) { svstra[7] = (32768 + (int)Math.Abs(SvAR[1, 2])).ToString(); } else { svstra[7] = SvAR[1, 2].ToString(); }
            if (SvAR[2, 2] < 0) { svstra[8] = (32768 + (int)Math.Abs(SvAR[2, 2])).ToString(); } else { svstra[8] = SvAR[2, 2].ToString(); }


            return true;
        }

    }
}
