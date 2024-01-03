# Haru.FileChecker

File integrity scanning.

### Implementation

It differs slightly from EFT's implementation; Instead of running the code
asynchronious, I opted for making it multi-threaded and significantly
simplify the implementation.

This results in 2-64x more throughput, depending on core count.
