﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drones.Utils
{
    public class TimeKeeper : MonoBehaviour
    {
        [SerializeField]
        private static TimeSpeed _TimeSpeed;
        public static TimeSpeed TimeSpeed
        {
            get
            {
                return _TimeSpeed;
            }
            set
            {
                _TimeSpeed = value;
            }
        }

        private static float _Degree;

        private static int _Day;

        private static int Hour
        {
            get
            {
                return (int)(_Degree / 360 * 24);
            }
        }

        private static int Minute
        {
            get
            {
                return (int)((_Degree / 360 * 24 - Hour) * 60);
            }
        }

        private static float Seconds
        {
            get
            {
                return ((_Degree / 360 * 24 - Hour) * 60 - Minute) * 60;
            }
        }

        void Awake()
        {
            transform.position = Vector3.up * 200;
            transform.eulerAngles = new Vector3(90, -90, -90);
            transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), 180);
            transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), 135); // 9am
            _Degree = 135;
        }

        private void FixedUpdate()
        {
            float speed;
            switch (TimeSpeed)
            {
                case TimeSpeed.Slow:
                    speed = 0.5f * 360.0f / (24 * 3600);
                    break;
                case TimeSpeed.Fast:
                    speed = 4 * 360.0f / (24 * 3600);
                    break;
                case TimeSpeed.Ultra:
                    speed = 8 * 360.0f / (24 * 3600);
                    break;
                case TimeSpeed.Pause:
                    speed = 0;
                    break;
                default:
                    speed = 360.0f / (24 * 3600);
                    break;
            }

            float dTheta = Time.fixedDeltaTime * speed;

            transform.RotateAround(Vector3.zero, new Vector3(0, 0, 1), dTheta);

            _Degree += dTheta;

            if (_Degree > 360)
            {
                _Day++;
                _Degree %= 360;
            }

        }

        public struct Chronos
        {
            private static uint _Count;
            private readonly int uid;
            int day;
            int hr;
            int min;
            float sec;
            public Chronos(int d, int h, int m, float s)
            {
                uid = (int)_Count++;
                day = d;
                hr = h;
                min = m;
                sec = s;
            }

            public override string ToString()
            {
                return string.Format("Day {0}, {1}:{2}", day, hr.ToString("00"), min.ToString("00"));
            }

            public string ToStringLong()
            {
                return ToString() + ":" + sec.ToString("00.000");
            }

            public static Chronos Get()
            {
                return new Chronos(_Day, Hour, Minute, Seconds);
            }

            public Chronos Now()
            {
                day = _Day;
                hr = Hour;
                min = Minute;
                sec = Seconds;
                return this;
            }

            public float Timer()
            {
                return (_Day - day) * 24 * 3600 + (Hour - hr) * 3600 + (Minute - min) * 60 + (Seconds - sec);
            }

            public override bool Equals(object obj)
            {
                return obj is Chronos && this == ((Chronos)obj);
            }

            public override int GetHashCode()
            {
                return uid;
            }

            public static bool operator <(Chronos t1, Chronos t2)
            {
                if (t1.day < t2.day)
                {
                    return true;
                }
                    
                if (t1.day == t2.day)
                {
                    if (t1.hr < t2.hr)
                    {
                        return true;
                    }
                    if (t1.hr == t2.hr)
                    {
                        if (t1.min < t2.min)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public static bool operator >(Chronos t1, Chronos t2)
            {
                if (t1.day > t2.day)
                {
                    return true;
                }

                if (t1.day == t2.day)
                {
                    if (t1.hr > t2.hr)
                    {
                        return true;
                    }
                    if (t1.hr == t2.hr)
                    {
                        if (t1.min > t2.min)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public static bool operator ==(Chronos t1, Chronos t2)
            {
                return t1.day == t2.day && t1.hr == t2.hr && t1.min == t2.min;
            }
            public static bool operator != (Chronos t1, Chronos t2)
            {
                return !(t1 == t2);
            }

        }

    }


}