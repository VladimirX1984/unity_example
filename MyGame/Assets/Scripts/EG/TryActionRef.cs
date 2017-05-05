namespace EG {
public delegate bool TryActionRef<T1>(ref T1 arg1);

public delegate bool TryActionRef<in T1, T2>(T1 arg1, ref T2 arg2);

public delegate bool TryActionRef<in T1, in T2, T3>(T1 arg1, T2 arg2, ref T3 args3);

public delegate bool TryActionRef<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, T3 args3,
    ref T4 args4);

public delegate bool TryActionRef<in T1, in T2, in T3, in T4, T5>(T1 arg1, T2 arg2, T3 args3,
    T4 args4, ref T5 args5);

public delegate bool TryActionRef<in T1, in T2, in T3, in T4, in T5, T6>(T1 arg1, T2 arg2, T3 args3,
    T4 args4, T5 args5, ref T6 args6);

public delegate bool TryActionRef<in T1, in T2, in T3, in T4, in T5, in T6, T7>(T1 arg1, T2 arg2,
    T3 args3, T4 args4, T5 args5, T6 args6, ref T7 args7);
}