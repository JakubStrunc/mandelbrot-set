namespace cv5
{
    public interface IComplex<T> where T: IComplex<T> 
    { 
        static abstract T operator +(T a, T b);


        static abstract T operator *(T a, T b);

        double NormSquared {  get; }
    }
}
