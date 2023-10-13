# JigsawExe

This project programatically captures the idea of smuggling and storing a malicious object in pieces to then be put together on site and then to be executed when ready. Since the target executable is broken up, it just looks like raw (even meaningless) data and can be tucked away somewhere on disk without much suspicion. Reassembly is very easy, however, because of how the pieces are broken up: similar to a linked list, each file's appended signature contains the MD5 checksum of the next file that connects to it, with `lem.myz` containing the number of pieces to the executeable and the head of the executable. With these two bits of information, you can piece back all the files. Conveniently, you only need to download `lem.myz` to then know every single file to download after that, so no additional filenames need to be permanently hard-coded to fetch.

This strategy works best when chained with encryption, so the pieces of the executable don't get recognized as such. So, you would first encrypt the executable as a whole, then use JigsawExe to break it up. You'd then use JigsawExe to put the pieces back together, then pass the whole encrypted binary to you decrypt function and then execute.

## Behavior
Just running the Python program will give this output:
```
$ python jigsaw.py          
[*] Usage: jigsaw.py <file> <byte_range>
```
With:
- `<file>`: is the file you want to break up.
- `<byte_range>`: is the size of each piece in bytes

The output will be `lem.myz` and a series of files named after MD5 checksums.