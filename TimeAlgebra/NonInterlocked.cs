namespace TimeAlgebra
{
    public class NonInterlocked
    {
        public static void Exchange<T>(ref T left, ref T right)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }
    }
}
