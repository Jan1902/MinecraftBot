using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MinecraftBot
{
    public class LocalPlayer
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }

        public bool OnGround { get => GetGroundHeight() == Y; }

        public Thread GravityThread;

        private Queue<Position> posQueue = new Queue<Position>();

        private double jumpStartY;
        private bool jumping;
        private long jumpStartTime;

        private double verticalVelocity;

        public LocalPlayer()
        {
            GravityThread = new Thread(GravityLoop);
        }

        public void Jump()
        {
            jumpStartY = Y;
            jumping = true;
            jumpStartTime = GetCurrentMillis();
        }

        private int GetGroundHeight()
        {
            var checkY = (int)Math.Round(Y) + 1;
            while (checkY > 0)
            {
                if (Client.Instance.World.IsSolid(new Pathfinding.Node((int)Math.Round(X - .5), checkY, (int)Math.Round(Z - .5))))
                {
                    return checkY + 1;
                }
                checkY--;
            }
            return -1;
        }

        public void GravityLoop()
        {
            while(true)
            {
                if (jumping)
                {
                    Y = jumpStartY + GetVerticalJumpPosition();
                }
                else
                {
                    var groundHeight = GetGroundHeight();
                    if (groundHeight == -1)
                        continue;

                    if (Y != groundHeight)
                    {
                        if (Y > groundHeight - verticalVelocity)
                        {
                            Y += verticalVelocity;
                            verticalVelocity -= 32 / 20;
                        }
                        else
                        {
                            Y = groundHeight;
                            verticalVelocity = 0;
                        }
                    }
                }

                Thread.Sleep(50);
            }
        }

        private long GetCurrentMillis()
        {
            return DateTime.Now.Hour * 60 * 60 * 1000 + DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
        }

        private double GetVerticalJumpPosition()
        {
            var x = (double)(GetCurrentMillis() - jumpStartTime - 250);
            x /= 1000;
            if (x > 0)
            {
                jumping = false;
                jumpStartY = 0;
                return 1.25;
            }
            return -20 * Math.Pow(x, 2) + 1.25;
        }

        public void LookAt(double x, double y, double z)
        {
            double dx = x - X;
            double dy = y - (Y + 1.6);
            double dz = z - Z;
            double r = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            float yaw = -(float)(Math.Atan2(dx, dz) / Math.PI * 180);
            if (yaw < 0)
                yaw = 360 + yaw;
            float pitch = (float)(-Math.Asin(dy / r) / Math.PI * 180);

            Yaw = yaw;
            Pitch = pitch;
        }

        public void Interpolate(double x, double y, double z)
        {
            if (Math.Abs(X - x) < .2)
            {
                X = x;
            }
            else
            {
                if (X < x)
                {
                    X += .2;
                }
                else
                {
                    X -= .2;
                }
            }

            if (Math.Abs(Z - z) < .2)
            {
                Z = z;
            }
            else
            {
                if (Z < z)
                {
                    Z += .2;
                }
                else
                {
                    Z -= .2;
                }
            }
        }

        //private void WalkQueueLoop()
        //{
        //    while (true)
        //    {
        //        if (posQueue.Count > 0)
        //        {
        //            var pos = posQueue.Dequeue();
        //            WalkLine(pos.X, pos.Y, pos.Z);
        //        }
        //        if (jumping)
        //        {
        //            Y = jumpStartY + GetVerticalJumpPosition();
        //        }
        //        Thread.Sleep(20);
        //    }
        //}

        //public void AddPosToQueue(double x, double y, double z)
        //{
        //    posQueue.Enqueue(new Position(x, y, z));
        //}

        //private void WalkLine(double x, double y, double z) //Could handle y differences but not used
        //{
        //    Client.Instance.SendChatMessage(string.Format("Now moving to {0} {1} {2}", x, y, z));
        //    if (y > Y)
        //    {
        //        yGoal = y;
        //        Interpolate();
        //        xGoal = x;
        //        zGoal = z;
        //        Interpolate();
        //    }
        //    else
        //    {
        //        xGoal = x;
        //        zGoal = z;
        //        Interpolate();
        //        yGoal = y;
        //        Interpolate();
        //    }
        //}
    }
}
