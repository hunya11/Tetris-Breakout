using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DXlib_CS.src.Comp.DrawComp.Image;
using System.Windows;

namespace DXlib_CS.src {
    static class Hit {

        /// <summary>
        ///　四角と円のあたり判定
        /// </summary>
        /// <param name="box"></param>
        /// <param name="circle"></param>
        /// <returns></returns>
        public static bool CheckBoxToCircle(DxImage box , DxImage circle) {

            //0:右上,1:右下,2:左下,3:左上
            Vector[] vec = new Vector[4];
            for(int i = 0 ; i < vec.Length ; i++) {
                vec[i] = new Vector();
            }

            vec[0].X = box.PosX + box.SizeX / 2;
            vec[0].Y = box.PosY - box.SizeY / 2;
            vec[1].X = box.PosX + box.SizeX / 2;
            vec[1].Y = box.PosY + box.SizeY / 2;
            vec[2].X = box.PosX - box.SizeX / 2;
            vec[2].Y = box.PosY + box.SizeY / 2;
            vec[3].X = box.PosX - box.SizeX / 2;
            vec[3].Y = box.PosY - box.SizeY / 2;

            //円の中に長方形の頂点が無いか
            for(int i = 0 ; i < vec.Length ; i++) {
                if(circle.Radius * circle.Radius > vec[i].X * vec[i].X + vec[i].Y * vec[i].Y) {
                    return true;
                }
            }

            //長方形の中に円が入り込んでい無いか
            Vector vecCircle = new Vector(circle.PosX , circle.PosY);

            Vector vec02 = Vector.Subtract(vec[1] , vec[0]);
            Vector vec0r = Vector.Subtract(vecCircle , vec[0]);
            double InnerProduct1 = Vector.Multiply(vec02 , vec0r);
            double OuterProduct1 = Vector.CrossProduct(vec02 , vec0r);
            double theta1 = Math.Atan2(OuterProduct1 , InnerProduct1);

            Vector vec13 = Vector.Subtract(vec[3] , vec[2]);
            Vector vec1r = Vector.Subtract(vecCircle , vec[2]);
            double InnerProduct2 = Vector.Multiply(vec13 , vec1r);
            double OuterProduct2 = Vector.CrossProduct(vec13 , vec1r);
            double theta2 = Math.Atan2(OuterProduct2 , InnerProduct2);

            if(0 <= theta1 && theta1 <= Math.PI / 2 && 0 <= theta2 && theta2 <= Math.PI / 2) {
                return true;
            }

            //長方形の辺と円の中心との距離

            for(int i = 0 ; i < vec.Length ; i++) {
                double dx, dy, a, b, t, tx, ty;
                double distance;
                dx = (vec[(i + 1) % 4].X - vec[i].X);
                dy = (vec[(i + 1) % 4].Y - vec[i].Y);
                a = dx * dx + dy * dy;
                b = dx * (vec[i].X - circle.PosX) + dy * (vec[i].Y - circle.PosY);
                t = -b / a;
                if(t < 0)
                    t = 0;
                if(t > 1)
                    t = 1;
                tx = vec[i].X + dx * t;
                ty = vec[i].Y + dy * t;
                distance = Math.Sqrt((circle.PosX - tx) * (circle.PosX - tx) + (circle.PosY - ty) * (circle.PosY - ty));

                if(distance < circle.Radius) {
                    return true;
                }
            }


            return false;
        }
    }
}
