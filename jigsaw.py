import sys
import hashlib

def writeBytes(bin: list, start: int, stop: int):
    byteSlice1 = bin[start:stop]
    testNulls = [b for b in byteSlice1 if b == 0]
    if len([b for b in byteSlice1 if b == 0]) == len(byteSlice1):
        print("[-] There exists at least one range given that has only null bytes. Vary range to be larger or smaller to fix this")
        exit(1)

    byteSlice2 = bin[stop:(stop + (stop - start))]
    md5 = hashlib.md5(byteSlice1).hexdigest()
    fileSignature = ''
    if len(byteSlice2) != 0:
        fileSignature= hashlib.md5(byteSlice2).hexdigest()
    with open(md5, "wb") as f:
        f.write(byteSlice1 + fileSignature.encode())
        f.close()
    return md5

def piece_back():
    with open("lemmy\.z", 'rb') as f:
        contents = f.read().split()
    numFiles = int(contents[0])
    curFile = contents[1].decode()
    buffer = b''
    for _ in range(numFiles - 1):
        try:
            with open(curFile, "rb") as f:
                stream = f.read()
            buffer += stream[:-32]
            curFile = stream[-32:].decode()
        except ValueError:
           print("[-] Encountered null bytes") 
           exit(1)

    with open(curFile, "rb") as f:
        buffer += f.read() 
    with open("original", "wb") as f:
        f.write(buffer)
        f.close()
        

def main():
    if len(sys.argv) < 3:
        print("[*] Usage: jigsaw.py <file> <byte_range>")
        exit(1)
    
    byterange = int(sys.argv[2])

    with open(sys.argv[1], 'rb') as f:
        contents = f.read()

    count = 0
        
    for i in range(0,len(contents), byterange):
        md5 = writeBytes(contents, i, i + byterange)
        if i == 0:
            init = md5
        count += 1

    with open("lemmy.z", 'w') as f:
        f.write(str(count) + ' ' + init)

    piece_back()

if __name__ == "__main__":
    main()
