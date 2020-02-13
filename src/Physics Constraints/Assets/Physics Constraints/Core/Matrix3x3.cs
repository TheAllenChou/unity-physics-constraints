/******************************************************************************/
/*
  Project - Physics Constraints
            https://github.com/TheAllenChou/unity-physics-constraints
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using UnityEngine;

namespace PhysicsConstraints
{
  public struct Matrix3x3
  {
    public float I00;
    public float I01;
    public float I02;
    public float I10;
    public float I11;
    public float I12;
    public float I20;
    public float I21;
    public float I22;

    public Vector3 Row0
    {
      get { return new Vector3(I00, I01, I02); }
      set { I00 = value.x; I01 = value.y; I02 = value.z; }
    }
    public Vector3 Row1
    {
      get { return new Vector3(I10, I11, I12); }
      set { I10 = value.x; I11 = value.y; I12 = value.z; }
    }
    public Vector3 Row2
    {
      get { return new Vector3(I20, I21, I22); }
      set { I20 = value.x; I21 = value.y; I22 = value.z; }
    }
    public Vector3 Col0
    {
      get { return new Vector3(I00, I10, I20); }
      set { I00 = value.x; I10 = value.y; I20 = value.z; }
    }
    public Vector3 Col1
    {
      get { return new Vector3(I01, I11, I21); }
      set { I01 = value.x; I11 = value.y; I21 = value.z; }
    }
    public Vector3 Col2
    {
      get { return new Vector3(I02, I12, I22); }
      set { I02 = value.x; I12 = value.y; I22 = value.z; }
    }

    public static Matrix3x3 FromRows(Vector3 row0, Vector3 row1, Vector3 row2)
    {
      return 
        new Matrix3x3
        (
          row0.x, row0.y, row0.z, 
          row1.x, row1.y, row1.z, 
          row2.x, row2.y, row2.z
        );
    }

    public static Matrix3x3 FromCols(Vector3 col0, Vector3 col1, Vector3 col2)
    {
      return 
        new Matrix3x3
        (
          col0.x, col1.x, col2.x, 
          col0.y, col1.y, col2.y, 
          col0.z, col1.z, col2.z
        );
    }

    public Matrix3x3(float i00, float i01, float i02, float i10, float i11, float i12, float i20, float i21, float i22)
    {
      I00 = i00; I01 = i01; I02 = i02;
      I10 = i10; I11 = i11; I12 = i12;
      I20 = i20; I21 = i21; I22 = i22;
    }

    public Matrix3x3 Inverted { get { return Inverse(this); } }
    public Matrix3x3 Transposed { get { return Transpose(this); } }

    private static readonly Matrix3x3 kIdentity = 
      new Matrix3x3
      (
        1.0f, 0.0f, 0.0f, 
        0.0f, 1.0f, 0.0f, 
        0.0f, 0.0f, 1.0f
      );
    public static Matrix3x3 Identity { get { return kIdentity; } }

    // Cross(A, B) = Skew(A) B = Skew(-B) A = Cross(-B, A)
    public static Matrix3x3 Skew(Vector3 v)
    {
      return
        new Matrix3x3
        (
            0.0f, -v.z,  v.y, 
           v.z,   0.0f, -v.x, 
          -v.y,  v.x,   0.0f
        );
    }

    public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
    {
      return FromRows(a.Row0 + b.Row0, a.Row1 + b.Row1, a.Row2 + b.Row2);
    }

    public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
    {
      return FromRows(a.Row0 - b.Row0, a.Row1 - b.Row1, a.Row2 - b.Row2);
    }

    public static Matrix3x3 operator *(float s, Matrix3x3 m)
    {
      return FromRows(s * m.Row0, s * m.Row1, s * m.Row2);
    }

    public static Matrix3x3 operator *(Matrix3x3 m, float s)
    {
      return s * m;
    }

    public static Vector3 operator *(Matrix3x3 m, Vector3 v)
    {
      return 
        new Vector3
        (
          Vector3.Dot(m.Row0, v), 
          Vector3.Dot(m.Row1, v), 
          Vector3.Dot(m.Row2, v)
        );
    }

    public static Vector3 operator *(Vector3 v, Matrix3x3 m)
    {
      return 
        new Vector3
        (
          Vector3.Dot(v, m.Col0), 
          Vector3.Dot(v, m.Col1), 
          Vector3.Dot(v, m.Col2)
        );
    }

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
      return
        FromCols
        (
          a * b.Col0, 
          a * b.Col1, 
          a * b.Col2
        );
    }

    public static float Mul(Vector3 a, Matrix3x3 m, Vector3 b)
    {
      return Vector3.Dot(a * m, b);
    }

    public static Matrix3x3 Inverse(Matrix3x3 m)
    {
      // too lazy to optimize
      // help, compiler
      float det = 
          m.I00 * m.I11 * m.I22 
        + m.I01 * m.I12 * m.I20 
        + m.I10 * m.I21 * m.I02 
        - m.I02 * m.I11 * m.I20 
        - m.I01 * m.I10 * m.I22 
        - m.I12 * m.I21 * m.I00;

      // I trust that this inertia tensor is well-constructed
      float detInv = 1.0f / det;

      return 
        new Matrix3x3
        (
          (m.I11 * m.I22 - m.I21 * m.I12) * detInv, 
          (m.I12 * m.I20 - m.I10 * m.I22) * detInv, 
          (m.I10 * m.I21 - m.I20 * m.I11) * detInv, 
          (m.I02 * m.I21 - m.I01 * m.I22) * detInv, 
          (m.I00 * m.I22 - m.I02 * m.I20) * detInv, 
          (m.I20 * m.I01 - m.I00 * m.I21) * detInv, 
          (m.I01 * m.I12 - m.I02 * m.I11) * detInv, 
          (m.I10 * m.I02 - m.I00 * m.I12) * detInv, 
          (m.I00 * m.I11 - m.I10 * m.I01) * detInv
        );
    }

    public static Matrix3x3 Transpose(Matrix3x3 m)
    {
      return
        new Matrix3x3
        (
          m.I00, m.I10, m.I20, 
          m.I01, m.I11, m.I21, 
          m.I02, m.I12, m.I22
        );
    }
  }
}
