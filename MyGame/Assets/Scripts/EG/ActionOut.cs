namespace EG {
public delegate void ActionOut<T1>(out T1 arg1);

public delegate void ActionOut<in T1, T2>(T1 arg1, out T2 arg2);

public delegate void ActionOut<in T1, in T2, T3>(T1 arg1, T2 arg2, out T3 args3);

public delegate void ActionOut<in T1, in T2, in T3, T4>(T1 arg1, T2 arg2, T3 args3, out T4 args4);

public delegate void ActionOut<in T1, in T2, in T3, in T4, T5>(T1 arg1, T2 arg2, T3 args3, T4 args4,
    out T5 args5);

public delegate void ActionOut<in T1, in T2, in T3, in T4, in T5, T6>(T1 arg1, T2 arg2, T3 args3,
    T4 args4, T5 args5, out T6 args6);

public delegate void ActionOut<in T1, in T2, in T3, in T4, in T5, in T6, T7>(T1 arg1, T2 arg2,
    T3 args3, T4 args4, T5 args5, T6 args6, out T7 args7);
}