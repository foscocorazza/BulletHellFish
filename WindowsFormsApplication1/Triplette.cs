using System.Collections.Generic;

namespace BulletHellFish
{
    class Triplette<A,B,C>
    {
        public A First;
        public B Second;
        public C Third;

        public Triplette(A first, B second, C third) {
            First = first;
            Second = second;
            Third = third;
        }

    }

    class TripletteList<A, B, C>
    {


        Dictionary<A, Triplette<A, B, C>> aDictionary;
        Dictionary<B, Triplette<A, B, C>> bDictionary;
        Dictionary<C, Triplette<A, B, C>> cDictionary;

        public TripletteList()
        {
            aDictionary = new Dictionary<A, Triplette<A, B, C>>();
            bDictionary = new Dictionary<B, Triplette<A, B, C>>();
            cDictionary = new Dictionary<C, Triplette<A, B, C>>();
        }


        public void Add(A a, B b, C c) {
            Triplette<A, B, C> triple = new Triplette<A, B, C>(a, b, c);

            aDictionary.Add(a, triple);
            bDictionary.Add(b, triple);
            cDictionary.Add(c, triple);
        }

        public Triplette<A, B, C> Get(A byFirst) {
            Triplette<A, B, C> triple = null;
            aDictionary.TryGetValue(byFirst, out triple);
            return triple;
        }

        public Triplette<A, B, C> Get(B bySecond)
        {
            Triplette<A, B, C> triple = null;
            bDictionary.TryGetValue(bySecond, out triple);
            return triple;
        }

        public Triplette<A, B, C> Get(C byThird)
        {
            Triplette<A, B, C> triple = null;
            cDictionary.TryGetValue(byThird, out triple);
            return triple;
        }

    }
}
