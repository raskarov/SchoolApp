Getting Started.

1) Your bootstrapper code should initialize the InterceptorProvider.
	e.g.    InterceptorProvider.SetInterceptorProvider(
                new DefaultInterceptorProvider(
                    new SoftDeleteChangeInterceptor()));

2) Your DbContext must inherit from DbContextBase.

3) Profit!