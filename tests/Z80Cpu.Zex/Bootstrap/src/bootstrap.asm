; Helper bootstrapper to load test .com file which uses couple CP/M functions
; to output the result to the screen. It is using OUT instruction to capture the
; output. This way zexdoc/zexall can be easily executed.

        DEVICE NOSLOT64K

        ORG $0000
        JP boot
        ORG $0005       ; fixed CP/M bdos entry point
        JP bdos


        ORG $0100       ; *.com program content will load at this address
prog:
        HALT            ; will be replaced with the *.com file

        ORG $F000       ; Boot and bdos routies implementation
boot:
        LD HL,exit
        LD ($0001),HL
        LD SP,$ffff     ; initiate stack to top memory address
        JP prog         ; and execute loaded program

bdos:
        LD A,C
        CP C_WRITE
        JR NZ,is_write_str
        LD A,E
        OUT (TEXT_OUT_PORT),A     ; will be captured by the handler
        RET

is_write_str:
        CP C_WRITESTR
        RET nz

write_str:
        LD A,(DE)
        CP '$'                   ; end of string?
        RET Z
        OUT (TEXT_OUT_PORT),A    ; will be captured by the handler
        INC DE
        JR write_str
        RET

exit:
        LD A,$A5
        OUT (EXIT_PORT),A
        HALT


C_WRITE = 2         ; BDOS function 2 (C_WRITE) - Console output
C_WRITESTR = 9      ; BDOS function 9 (C_WRITESTR) - Output string
TEXT_OUT_PORT = 5   ; port used to output text
EXIT_PORT = 6       ; port used to detect end of tests

        ; create boot.com file
        savebin "bootstrap.bin", $0000, $FFFF
